using ASP_CQRS.Application.Functions.Posts;
using ASP_CQRS.Application.Functions.Posts.Commands.CreatePost;
using ASP_CQRS.Application.Functions.Posts.Commands.DeletePost;
using ASP_CQRS.Application.Functions.Posts.Commands.UpdatePost;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ASP_CQRS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name ="GetAllPosts")]
        public async Task<ActionResult<List<PostInListViewModel>>> GetAllPosts()
        {
            var list = await _mediator.Send(new GetPostDetailQuery());
            return Ok(list);
        }

        [HttpGet("{id}", Name ="GetPostById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PostDetailViewModel>> GetPostById(int id)
        {
            var detailViewModel = await _mediator.Send(new GetPostDetailQuery() { Id = id });
            return Ok(detailViewModel);
        }

        [HttpPost(Name = "AddPost")]
        public async Task<ActionResult<int>> Create([FromBody] CreatedPostCommand createdPostCommand) 
        {
            var result = await _mediator.Send(createdPostCommand);
            return Ok(result.PostId);
        }

        [HttpPut(Name = "UpdatePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdatePostCommand updatePostCommand)
        {
            await _mediator.Send(updatePostCommand);
            return NoContent();
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var deletepostCommand = new DeletePostCommand() { PostId = id };
            await _mediator.Send(deletepostCommand);
            return NoContent();
        }
    }
}
