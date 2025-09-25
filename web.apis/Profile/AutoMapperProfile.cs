using common.data.Enums;
using common.data.Extensions;
using data.models;
using web.apis.BindingModels;
using web.apis.Models;
using web.apis.ViewModels;

namespace web.apis.Profile
{
    public class AutoMapperProfile : AutoMapper.Profile
	{
        public AutoMapperProfile()
        {

            CreateMap<UserRegistrationBindingModel, ApplicationUser>()
              .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
              .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
              .ForMember(dest => dest.PhoneNumber, opts => opts.MapFrom(src => src.MobileNumber))
              .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.EmailAddress))
              .ForMember(dest => dest.NormalizedEmail, opts => opts.MapFrom(src => src.EmailAddress.ToUpper()))
              .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.EmailAddress))
              .ForMember(dest => dest.NormalizedUserName, opts => opts.MapFrom(src => src.EmailAddress.ToUpper()))
              // .ForMember(dest => dest.UserRoleEnum, opts => opts.MapFrom(src => UserRoleEnum.IdeaOwner))
              .ForMember(dest => dest.OrganisationName, opts => opts.MapFrom(src => src.OrganisationName));

            CreateMap<ApplicationUser, UserViewModel>()
              .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
              .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.FirstName))
              .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.LastName))
              .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
              .ForMember(dest => dest.EmailAddress, opts => opts.MapFrom(src => src.Email))
              .ForMember(dest => dest.PhoneNumber, opts => opts.MapFrom(src => src.PhoneNumber))
              .ForMember(dest => dest.OrganisationName, opts => opts.MapFrom(src => src.OrganisationName))
              .ForMember(dest => dest.UserRole, opts => opts.MapFrom(src => src.UserRoleEnum.GetDisplayName()))
              .ForMember(dest => dest.IsActive, opts => opts.MapFrom(src => src.IsActive));

            CreateMap<Notification, NotificationViewModel>().ReverseMap(); //reverse so the both direction            

            #region Application User View Model
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap(); //reverse so the both direction
            CreateMap<ApplicationUser, InvestorViewModel2>().ReverseMap(); //reverse so the both direction
            #endregion

            #region Subscription
            CreateMap<SubscriptionBindingModel, Subscription>().ReverseMap();
            CreateMap<SubscriptionUpdateBindingModel, Subscription>().ReverseMap();
            CreateMap<Subscription, SubscriptionViewModel>().ReverseMap();
            CreateMap<UserSubscription, UserSubscriptionViewModel>().ReverseMap();
            #endregion

            #region Document
            CreateMap<DocumentBindingModel, Document>().ReverseMap();
            CreateMap<DocumentUpdateBindingModel, Document>().ReverseMap();
            CreateMap<Document, DocumentViewModel>().ReverseMap();
            #endregion

            #region Email Template
            CreateMap<EmailTemplateBindingModel, EmailTemplate>().ReverseMap();
            CreateMap<EmailTemplateUpdateBindingModel, EmailTemplate>().ReverseMap();
            CreateMap<EmailTemplate, EmailTemplateViewModel>().ReverseMap();
            #endregion

            #region Promo Code
            CreateMap<PromoCodeBindingModel, PromoCode>().ReverseMap();
            CreateMap<PromoCodeUpdateBindingModel, PromoCode>().ReverseMap();
            CreateMap<PromoCode, PromoCodeViewModel>().ReverseMap();
            #endregion

            #region Campaign
            CreateMap<CampaignBindingModel, Campaign>().ReverseMap();
            CreateMap<CampaignUpdateBindingModel, Campaign>().ReverseMap();
            CreateMap<Campaign, CampaignViewModel>().ReverseMap();
            #endregion
        }
    }
}

