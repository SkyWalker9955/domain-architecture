using Marketplace.Contracts;
using Marketplace.Domain;
using Marketplace.Framework;

namespace Marketplace.Api;
public class ClassifiedAdApplicationService : IApplicationService
{
    private readonly IClassifiedAdRepository _repository;
    private ICurrencyLookup _currencyLookup;

    public ClassifiedAdApplicationService(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup)
    {
        _repository = repository;
        _currencyLookup = currencyLookup;
    }

    public Task Handle(Object command) =>
        command switch
        {
            ClassifiedAds.V1.Create cmd => HandleCreate(cmd),
            ClassifiedAds.V1.SetTitle cmd => HandleUpdate(cmd.Id, c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))),
            ClassifiedAds.V1.UpdateText cmd => HandleUpdate(cmd.Id, c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text))),
            ClassifiedAds.V1.UpdatePrice cmd => HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
            ClassifiedAds.V1.RequestToPublish cmd => HandleUpdate(cmd.Id, c => c.RequestToPublish()),
            _ => Task.CompletedTask
        };
    private async Task HandleCreate(ClassifiedAds.V1.Create cmd)
    {
        if (await _repository.Exists(cmd.Id.ToString()))
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

        var classifiedAd = new ClassifiedAd(
            new ClassifiedAdId(cmd.Id),
            new UserId(cmd.OwnerId)
        );

        await _repository.Save(classifiedAd);
    }

    private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
    {
        var classifiedAd = _repository.Load(classifiedAdId.ToString());
        if (classifiedAd == null)
        {
            throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
        }

        operation(await classifiedAd);
        await _repository.Save(await classifiedAd);
    }
}