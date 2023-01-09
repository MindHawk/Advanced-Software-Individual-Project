namespace PostServiceModels.Interfaces;

public interface IPostMessageBusLogic
{
    public bool AddForum(Forum forum);
    public bool DeleteForum(Forum forum);
    public bool AddAccount(Account account);
}