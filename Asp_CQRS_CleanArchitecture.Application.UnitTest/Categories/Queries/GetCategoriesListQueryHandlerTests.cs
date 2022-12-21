using AutoMapper;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Application.Functions.Categories.Queries.GetCategoryList;
using ASP_CQRS.Application.Mapper;
using Asp_CQRS_CleanArchitecture.Application.UnitTest.Mocks;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Asp_CQRS_CleanArchitecture.Application.UnitTest.Categories.Queries
{
    public class GetCategoriesListQueryHandlerTests
    {
        private readonly IMapper mapper;
        private readonly Mock<ICategoryRepository> mockCategoryRepository;

        public GetCategoriesListQueryHandlerTests()
        {
            mockCategoryRepository = RepositioryMocks.GetCategoryRepository();
            var configurationProvider = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                }
            );
            mapper = configurationProvider.CreateMapper();
        }

        [Fact]
        public async Task GetCategoriesListTest() 
        {
            var handler = new GetCategoriesListQueryHandler(this.mapper, this.mockCategoryRepository.Object);
            var result = await handler.Handle(new GetCategoriesListQuery(), CancellationToken.None);
            result.ShouldNotBeOfType<List<CategoryInListViewModel>>();
            result.Count.ShouldBe(5);
        }
    }
}
