using System;
using System.Globalization;

namespace Marketplace.Domain
{
    public class ClassifiedAd
    {
        public ClassifiedAdId Id { get; }
        public UserId OwnerId { get; }
        public ClassifiedAdTitle Title { get; private set; }
        public ClassifiedAdText Text { get; private set; }
        public Price Price { get; private set; }
        public ClassifiedAdState State { get; private set; }
        public UserId ApprovedBy { get; private set; }
        public enum ClassifiedAdState
        {
            PendingReview,
            Active,
            Inactive,
            MarkedAsSold
        }

        public ClassifiedAd(ClassifiedAdId id, UserId ownerId)
        {
            Id = id;
            OwnerId = ownerId;
            State = ClassifiedAdState.Inactive;
            EnsureValidState();
        }

        public void SetTitle(ClassifiedAdTitle title)
        {
            Title = title;
            EnsureValidState();
        }

        public void UpdateText(ClassifiedAdText text)
        {
            Text = text;
            EnsureValidState();
        }

        public void UpdatePrice(Price price)
        {
            Price = price;
            EnsureValidState();
        }

        public void RequestToPublish()
        {
            State = ClassifiedAdState.PendingReview;
            EnsureValidState();
        }

        private void EnsureValidState()
        {
            var valid = Id != null &&
                        OwnerId != null &&
                        (State switch
                        {
                            ClassifiedAdState.PendingReview =>
                                Title != null
                                && Text != null
                                && Price?.Amount > 0,
                            ClassifiedAdState.Active =>
                                Title != null
                                && Text != null
                                && Price?.Amount > 0
                                && ApprovedBy != null,
                            _ => true
                        });
            if (!valid)
                throw new InvalidEntityStateException(
                    this, $"Post-checks failed in state {State}");
        }

        private UserId _ownerId;
        private string _title;
        private string _text;
        private decimal _price;
    }

    public class InvalidEntityStateException : Exception
    {
        public InvalidEntityStateException(object entity, string message) 
            : base($"Entity {entity.GetType().Name} state change rejected, {message}")
        {
        }
    }
    
}