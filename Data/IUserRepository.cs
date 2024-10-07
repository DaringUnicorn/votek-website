
namespace votek.Data{

    public interface IUserRepository{

        IQueryable<User> Users { get; }

        void  CreateUser(User user);

    }
}