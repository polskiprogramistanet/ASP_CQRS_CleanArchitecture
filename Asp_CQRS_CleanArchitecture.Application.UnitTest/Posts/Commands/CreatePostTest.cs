using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Moq;
using Shouldly;
using Xunit;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Application.Functions.Posts.Commands.CreatePost;
using ASP_CQRS.Application.Mapper;
using Asp_CQRS_CleanArchitecture.Application.UnitTest.Mocks;

namespace Asp_CQRS_CleanArchitecture.Application.UnitTest.Posts.Commands
{
    public class CreatePostTest
    {
        private readonly IMapper mapper;
        private readonly Mock<IPostRepository> mockPostRepository;
        public CreatePostTest()
        {
            this.mockPostRepository = RepositioryMocks.GetPostRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                }
            );
            this.mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidPost_AddedToPostRepo()
        {
            var handler = new CreatedPostCommandHandler(this.mockPostRepository.Object, this.mapper);
            var allPostsBeforeCount = (await this.mockPostRepository.Object.GetAllItems()).Count;
            var command = new CreatedPostCommand()
            {
                Title = "Stefan Papieć",
                Date = DateTime.Now.AddDays(-14),
                Rate = 9,
                Author = "Stefan Papieć"
            };

            var response = await handler.Handle(command, CancellationToken.None);
            var allPosts = await this.mockPostRepository.Object.GetAllItems();

            response.Success.ShouldBe(true);
            response.ValidationErrors.Count.ShouldBe(0);
            allPosts.Count.ShouldBe(allPostsBeforeCount + 1);
            response.PostId.ShouldNotBeNull();
        }
        [Fact]
        public async Task Handle_Not_ValidPost_ToonLongTitle_81Characters_NotAddedToPostRepo()
        {
            var handler = new CreatedPostCommandHandler(this.mockPostRepository.Object, this.mapper);
            var allPostsBeforeCount = (await this.mockPostRepository.Object.GetAllItems()).Count;
            var command = new CreatedPostCommand()
            {
                Title = new string('*', 81),
                Date = DateTime.Now.AddDays(-14),
                Rate = 9,
                Author = "Marian Paździoch"
            };
            var response = await handler.Handle(command, CancellationToken.None);
            var allPosts = await this.mockPostRepository.Object.GetAllItems();

            response.Success.ShouldBe(false);
            response.ValidationErrors.Count.ShouldBe(1);
            allPosts.Count.ShouldBe(allPostsBeforeCount);
            response.PostId.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_Not_ValidPost_FutureDate_2DayInToTheFuture_NotAddedToPostRepo()
        {
            var handler = new CreatedPostCommandHandler(this.mockPostRepository.Object, this.mapper);
            var allPostsBeforeCount = (await this.mockPostRepository.Object.GetAllItems()).Count;
            var command = new CreatedPostCommand()
            {
                Title = new string('*', 80),
                Date = DateTime.Now.AddDays(2),
                Rate = 9,
                Author = "Tadeusz Guć"
            };
            var response = await handler.Handle(command, CancellationToken.None);
            var allPosts = await this.mockPostRepository.Object.GetAllItems();

            response.Success.ShouldBe(false);
            response.ValidationErrors.Count.ShouldBe(1);
            allPosts.Count.ShouldBe(allPostsBeforeCount);
            response.PostId.ShouldBeNull();
        }
        [Fact]
        public async Task Handle_Not_ValidPost_RateToBig_NotAddedToPostRepo()
        {
            var handler = new CreatedPostCommandHandler(this.mockPostRepository.Object, this.mapper);
            var allPostsBeforeCount = (await this.mockPostRepository.Object.GetAllItems()).Count;
            var command = new CreatedPostCommand()
            {
                Title = new string('*', 80),
                Date = DateTime.Now.AddDays(-12),
                Rate = 101,
                Author = "Andrzej"
            };
            var response = await handler.Handle(command, CancellationToken.None);
            var allPosts = await this.mockPostRepository.Object.GetAllItems();
            response.Success.ShouldBe(false);
            response.ValidationErrors.Count.ShouldBe(1);
            allPosts.Count.ShouldBe(allPostsBeforeCount);
            response.PostId.ShouldBeNull();
        }
    }
}
