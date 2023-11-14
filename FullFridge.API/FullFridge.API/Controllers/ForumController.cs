using FullFridge.API.Models;
using FullFridge.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FullFridge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IForumService _forumService;
        public ForumController(IForumService forumService)
        {
            _forumService = forumService;
        }


        //POST: api/Forum
        [HttpPost]
        public async Task<ActionResult> PostContent(Post post)
        {
            await _forumService.AddPost(post);
            return Ok();
        }

        //POST: api/Forum/Comment
        [HttpPost("Comment")]
        public async Task<ActionResult> PostComment(PostComment comment)
        {
            await _forumService.AddPostComment(comment);
            return Ok();
        }

        //GET: api/Forum
        [HttpGet]
        public async Task<ActionResult<PostList>> GetPosts(int page, int pageSize)
        {
            int skipCount = (page - 1) * pageSize;
            var posts = await _forumService.GetAllPosts(skipCount, pageSize);

            return Ok(posts);
        }

        //GET: api/Forum/{id}/Comments
        [HttpGet("{postId}/Comments")]
        public async Task<ActionResult<IEnumerable<PostCommentDTO>>> GetComments(Guid postId)
        {
            var comments = await _forumService.GetComments(postId);

            return Ok(comments);
        }

        //GET: api/Forum/{id}
        [HttpGet("{postId}")]
        public async Task<ActionResult<PostDTO>> GetPostDetails(Guid postId)
        {
            var post = await _forumService.GetPostDetails(postId);

            return Ok(post);
        }
    }
}
