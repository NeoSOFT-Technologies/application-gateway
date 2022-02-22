using ApplicationGateway.Application.Responses;
using MediatR;

namespace ApplicationGateway.Application.Features.Api.Commands.CreateMultipleApisCommand
{
    public class CreateMultipleApisCommand : IRequest<Response<CreateMultipleApisDto>>
    {
        public List<MultipleApiModel> APIs { get; set; }
    }

    public class MultipleApiModel
    {
        public string Name { get; set; }
        public string ListenPath { get; set; }
        public string TargetUrl { get; set; }
    } 
}
