using System.Windows.Input;
using Marketplace.Contracts;
using Marketplace.Domain;
using Marketplace.Framework;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api;

[Tags("ClassifiedAds")]
[Route("/ad")]
public class ClassifiedAdsCommandsApi : Controller
{
    private readonly ClassifiedAdApplicationService _applicationService;

    public ClassifiedAdsCommandsApi(
        ClassifiedAdApplicationService applicationService)
        => _applicationService = applicationService;

    [HttpPost]
    public async Task<IActionResult> Post(ClassifiedAds.V1.Create request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("name")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAds.V1.SetTitle request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("text")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAds.V1.UpdateText request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }

    [Route("price")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAds.V1.UpdatePrice request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }
    
    [Route("publish")]
    [HttpPut]
    public async Task<IActionResult> Put(ClassifiedAds.V1.RequestToPublish request)
    {
        await _applicationService.Handle(request);
        return Ok();
    }
}