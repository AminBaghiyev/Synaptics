namespace Synaptics.Domain.Enums;

public enum MessageCode
{
    SomethingWrong,
    TokenNotFound,
    CredentialsWrong,
    UserNotExists,
    UserExistsThisEmail, 
    UserExistsThisUserName,
    YouCantFollowYourself,
    YouDoNotFollowYourselfAnyway,
    PostNotExists,
    YouArentAllowedEditPost,
    YouArentAllowedDeletePost,
    YouArentAllowedRecoverPost,
    YouAlreadyLikedPost,
    YouArentLikedPost,
    YouAlreadyFollowUser,
    YouArentFollowUser,
    UserIsntFollowYou,
    UserHasNoPosts,
    CommentNotExists,
    YouAlreadyLikedComment,
    YouArentAllowedEditComment,
    YouArentAllowedDeleteComment,
    YouArentLikedComment,
}
