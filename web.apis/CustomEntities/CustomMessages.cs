using System;
namespace web.apis
{
	public static class CustomMessages
	{
		public static string Saved(string entityName) => $"{entityName} has been successfully added";        
        public static string NotSaved(string entityName) => $"{entityName} has NOT been added";
        public static string Updated(string entityName) => $"{entityName} has been successfully updated";
        public static string NotUpdated(string entityName) => $"{entityName} was not successful";
        public static string Deleted(string entityName) => $"{entityName} has been successfully deleted";
        public static string Fetched(string count, string entityName) => $"{count} {entityName} found";
        public static string NotOwn(string entityName) => $"user does not own {entityName}";
        public static string NotFound(string entityName) => $"{entityName} not found";
        public static string Invalid() => $"Some field(s) are not valid";
        public static string StringMessage(string message) => $"{message}";
        public static string Uploaded(string entityName) => $"{entityName} has been successfully uploaded";
        public static string NotUploaded(string entityName) => $"{entityName} has NOT been uploaded";
        public static string NoInstitution() => $"User does not belong to an institution";
        public static string ExpiredLink() => "Invite complete registration link is no longer valid. Please contact Learning Institution";
        public static string NoResult() => "There are no results.";
        public static string NotAuthorised() => "You are not authorised to perform this action.";
        public static string DoesNotOwnProfile(string entityName) => "User does not own profile";
        public static string InvalidCredential() => "Invalid Credentials";
    }
}

