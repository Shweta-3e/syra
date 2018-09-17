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

            Mapper.CreateMap<Customer, CustomerView>();
            Mapper.CreateMap<CustomerView, Customer>();

            Mapper.CreateMap<CustomerPlan, CustomerPlanView>()
                  .ForMember(c => c.AllowedBotLimit, b => b.MapFrom(s => s.Plan.AllowedBotLimit))
                  .ForMember(c => c.PlanName, b => b.MapFrom(s => s.Plan.Name)); 
            Mapper.CreateMap<CustomerPlanView, CustomerPlan>();
        }
    }
}