using MediatR;
using web.apis.Queries;
using ILogger = Serilog.ILogger;
using data.models;
using web.apis.Models;
using Microsoft.AspNetCore.Identity;
using common.data.Enums;

namespace web.apis.Handlers
{
    public class UserDetailsHandler : IRequestHandler<NewUserQuery, UserDetails>
	{
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly DbConn _dbConn;

        public UserDetailsHandler(ILogger logger,
            IConfiguration configuration, IEmailRepository emailRepository, UserManager<ApplicationUser> userManager,
            ISubscriptionRepository subscriptionRepository, DbConn dbConn)
		{
            _logger = logger;
            _configuration = configuration;
            _emailRepository = emailRepository;
            _userManager = userManager;
            _subscriptionRepository = subscriptionRepository;
            _dbConn = dbConn;
        }

        public async Task<UserDetails> Handle(NewUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var reminderDays = _configuration.GetValue<int>("Reminder:Days");
                var userId = request._model.UserId;
                var userDetail = request._model;

                if(string.IsNullOrWhiteSpace(userId))
                    throw new Exception("UserId is null or empty and this should not be");

                var user = await _userManager.FindByIdAsync(userId);
                
                // if(user.UserRoleEnum == UserRoleEnum.Lecturer || user.UserRoleEnum == UserRoleEnum.Student)
                // {
                //     var learningInstitutionId = 0;
                //     var courseId = 0;
                //     if(user.LearningInstitutionId.HasValue)
                //     {
                //         learningInstitutionId = user.LearningInstitutionId.Value;
                //         courseId = userDetail.CourseId;
                // 
                //     } else
                //     {
                //         learningInstitutionId = userDetail.LearningInstitutionId;
                //         courseId = userDetail.CourseId;
                //     }                
                // }

                // await _dbConn.SaveChangesAsync(userId);
                
                // add user subscription
                Subscription subscription;
                if (request.IsInvestor)
                {
                    subscription = await _subscriptionRepository.GetInvestorSubscription();
                } else
                {
                    if (request._model.SubscriptionId == 0)
                        request._model.SubscriptionId = 1;

                    subscription = await _subscriptionRepository.GetSingle(request._model.SubscriptionId);
                }

                // if(user.UserRoleEnum == UserRoleEnum.Student
                //     || user.UserRoleEnum == UserRoleEnum.LearningInstitution
                //     || user.UserRoleEnum == UserRoleEnum.Lecturer
                //     || user.UserRoleEnum == UserRoleEnum.Institution)
                // {
                //     subscription = await _subscriptionRepository.GetLISubscription();
                // }
                
                if (subscription == null)
                    throw new Exception("Subscription not found");

                var addedSubscription = await _subscriptionRepository.AddUserSubscription(subscription, request._model.UserId, request.LoggedInUserId);
                if (addedSubscription == null)
                    throw new Exception("User Subscription could not be added");

                // log email to be sent 
                await _emailRepository.SendEmail(user.FirstName, user.Email, "new_registration");

                // log reminder to be sent
                var message = $"<p>Good day {request._model.FirstName}, <br/><br/> Welcome to Hydreate. We are delighted you are here and we cannot wait to help nurture your ideas. <br/><br/> Happy brainstorming. <br/><br/> Hydreate <br/><b>Welcome Team</b> </p>";

                return request._model;
            }
            catch (Exception ex)
            {
                _logger.Error("", ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}

