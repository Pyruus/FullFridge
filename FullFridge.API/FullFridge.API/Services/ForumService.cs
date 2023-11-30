using FullFridge.API.Models;
using FullFridge.API.Models.Enums;
using FullFridge.Model;
using FullFridge.Model.Helpers;

namespace FullFridge.API.Services
{
    public class ForumService: IForumService
    {
        private readonly IDapperRepository _repository;

        public ForumService(IDapperRepository repository)
        {
            _repository = repository;
        }

        public async Task AddPost(Post post)
        {
            await _repository.Execute(
                SqlQueryHelper.InsertPost, new 
                {
                    post.Title,
                    post.Content,
                    post.CreatedBy,
                    post.RecipeId
                });
        }

        public async Task AddPostComment(PostComment comment)
        {
            await _repository.Execute(
                SqlQueryHelper.InsertPostComment, new 
                {
                    comment.PostId,
                    comment.Content,
                    comment.CreatedBy
                });
        }

        public async Task<PostList> GetAllPosts(int skip, int take)
        {
            var posts = await _repository.Query<Post>(
                SqlQueryHelper.GetPosts);

            var postsList = new PostList()
            {
                Posts = posts.Skip(skip).Take(take).ToList(),
                Pages = (int)Math.Ceiling((decimal)posts.Count() / (decimal)take)
            };

            return postsList;
        }

        public async Task<IEnumerable<PostCommentDTO>> GetComments(Guid postId)
        {
            return await _repository.Query<PostCommentDTO>(
                SqlQueryHelper.GetPostComments, new { postId });
        }

        public async Task<PostDTO> GetPostDetails(Guid id)
        {
            var post = await _repository.QueryFirstOrDefault<Post>(
                SqlQueryHelper.GetPostById, new { id });

            var user = await _repository.QueryFirstOrDefault<UserDTO>(
                SqlQueryHelper.GetUserDetails, new { id = post.CreatedBy });

            return new PostDTO(post, user);
        }

        public async Task<Result> DeletePost(Guid id, Guid userId)
        {
            var post = await _repository.QueryFirstOrDefault<Post>(
                SqlQueryHelper.GetPostById, new { id });

            if (post == null) return new Result(StatusCodes.Status404NotFound, "Post not found");

            var userRole = await _repository.QueryFirstOrDefault<string>(
                SqlQueryHelper.GetUserRole, new { id = userId });

            if (userRole != Roles.RoleAdmin && post.CreatedBy != userId) return new Result(StatusCodes.Status403Forbidden, "You are not a creator of the post");

            await _repository.Execute(
                SqlQueryHelper.DeletePost, new { id });
            await _repository.Execute(
                SqlQueryHelper.DeletePostComments, new { id });

            return new Result(StatusCodes.Status200OK);
        }

        public async Task<Result> DeletePostComment(Guid id, Guid userId)
        {
            var post = await _repository.QueryFirstOrDefault<PostComment>(
                SqlQueryHelper.GetPostCommentById, new { id });

            if (post == null) return new Result(StatusCodes.Status404NotFound, "Recipe not found");

            var userRole = await _repository.QueryFirstOrDefault<string>(
                SqlQueryHelper.GetUserRole, new { id = userId });

            if (userRole != Roles.RoleAdmin && post.CreatedBy != userId) return new Result(StatusCodes.Status403Forbidden, "You are not a creator of the comment");

            await _repository.Execute(
                SqlQueryHelper.DeleteSinglePostComment, new { id });

            return new Result(StatusCodes.Status200OK);
        }
    }

    public interface IForumService
    {
        Task AddPost(Post post);
        Task AddPostComment(PostComment comment);
        Task<PostList> GetAllPosts(int skip, int take);
        Task<IEnumerable<PostCommentDTO>> GetComments(Guid postId);
        Task<PostDTO> GetPostDetails(Guid id);
        Task<Result> DeletePost(Guid id, Guid userId);
        Task<Result> DeletePostComment(Guid id, Guid userId);
    }
}
