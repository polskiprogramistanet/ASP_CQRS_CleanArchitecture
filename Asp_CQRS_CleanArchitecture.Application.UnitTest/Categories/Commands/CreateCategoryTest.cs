using AutoMapper;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Application.Functions.Categories.Commands.CreateCategory;
using ASP_CQRS.Application.Mapper;
using Asp_CQRS_CleanArchitecture.Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Asp_CQRS_CleanArchitecture.Application.UnitTest.Categories.Commands
{
    public class CreateCategoryTest
    {
        private readonly IMapper mapper;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;

        public CreateCategoryTest()
        {
            this.mockCategoryRepository = RepositioryMocks.GetCategoryRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task Handle_ValidCategory_AddedToCategoriesRepo()
        {
            var handler = new CreatedCategoryCommandHandler(mockCategoryRepository.Object, mapper);
            var allCategoriesBeforeCount = (await mockCategoryRepository.Object.GetAllItems()).Count;
            var response = await handler.Handle(new CreatedCategoryCommand()
            { Name = "Test", DisplayName = "Test" }, CancellationToken.None);
            var allCategories = await mockCategoryRepository.Object.GetAllItems();
            response.Success.ShouldBe(true);
            response.ValidationErrors.Count.ShouldBe(0);
            allCategories.Count.ShouldBe(allCategoriesBeforeCount + 1);
            response.CategoryId.ShouldNotBeNull();
        }
    }
}
