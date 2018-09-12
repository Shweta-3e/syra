using AutoMapper;
using Syra.Admin.Entities;
using Syra.Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Syra.Admin.Helper
{
    public class AutoMapperConfig
    {
        public static void Map()
        {
            Mapper.CreateMap<BotDeployment, BotDeploymentView>()
                .ForMember(c => c.LuisDomainName, b => b.MapFrom(s => s.LuisDomain.Name));
            Mapper.CreateMap<BotDeploymentView, BotDeployment>();

            Mapper.CreateMap<Message, MessageView>();
            Mapper.CreateMap<MessageView, Message>();

            Mapper.CreateMap<BotQuestionAnswers, BotQuestionAnswersView>();
            Mapper.CreateMap<BotQuestionAnswersView, BotQuestionAnswers>();
        }
    }
}