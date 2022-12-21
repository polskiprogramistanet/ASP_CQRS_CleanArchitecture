using AutoMapper;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Application.Functions.Webinars.Commands.CreateWebinar;
using ASP_CQRS.Application.Mapper;
using Asp_CQRS_CleanArchitecture.Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Asp_CQRS_CleanArchitecture.Application.UnitTest.Webinars.Commands
{
    public class CreateWebinarTest
    {
        private readonly IMapper mapper;
        private readonly Mock<IWebinaryRepository> mockWebinarRepository;

        public CreateWebinarTest()
        {
            this.mockWebinarRepository = RepositioryMocks.GetWebinarRepository();
            var configurationProvider = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<MappingProfile>();
            });
            mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidWebinar_AddedToWebinarRepo()
        {
            var handler = new CreateWebinarCommandHandler(mockWebinarRepository.Object, mapper);
            var allWebinarsBeforeCount = (await mockWebinarRepository.Object.GetAllItems()).Count;
            var command = new CreateWebinarCommand()
            {
                ImageUrl = "Testtest",
                Title = new string('*', 80),
                FacebookEventUrl = "TestTest",
                Date = DateTime.Now.AddDays(-14)
            };
            var response = await handler.Handle(command, CancellationToken.None);
            var allWebinars = await mockWebinarRepository.Object.GetAllItems();

            response.Success.ShouldBe(true);
            response.ValidationErrors.Count.ShouldBe(0);
            allWebinars.Count.ShouldBe(allWebinarsBeforeCount + 1);
            response.Id.ShouldNotBeNull();
        }
    }
}
