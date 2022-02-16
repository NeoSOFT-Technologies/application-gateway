﻿using ApplicationGateway.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationGateway.Application.Features.Transformer.Commands.UpdateTransformerCommand
{
    public class UpdateTransformerCommand : IRequest<Response<UpdateTransformerDto>>
    {
        public Guid Id { get; set; }

        public string TemplateName { get; set; }

        public string TransformerTemplate { get; set; }

        public string Gateway { get; set; }

    }
}
