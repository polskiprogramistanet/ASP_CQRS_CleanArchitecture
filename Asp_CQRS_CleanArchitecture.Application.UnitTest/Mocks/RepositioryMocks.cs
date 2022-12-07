using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ASP_CQRS.Application.Common;
using ASP_CQRS.Application.Contracts.Persistence;
using ASP_CQRS.Domain.Entities;
using Moq;

namespace Asp_CQRS_CleanArchitecture.Application.UnitTest.Mocks
{
    public class RepositioryMocks
    {
        public static Mock<ICategoryRepository> GetCategoryRepository()
        {
            var categories = GetCategories();
            var mockCategoryRepository = new Mock<ICategoryRepository>();

            mockCategoryRepository.Setup(repo => repo.GetAllItems()).ReturnsAsync(categories);

            mockCategoryRepository.Setup(repo => repo.GetByIdItem(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                    {
                        var cat = categories.FirstOrDefault(c => c.CategoryId == id);
                        return cat;
                    }
                );
            mockCategoryRepository.Setup(repo => repo.DeleteItem(It.IsAny<Category>())).Callback<Category>((entity) => categories.Remove(entity));

            mockCategoryRepository.Setup(repo => repo.UpdateItem(It.IsAny<Category>())).Callback<Category>((entity) => {categories.Remove(entity);categories.Add(entity);});

            var categoriesWithPost = GetCategoriesWithPosts();

            mockCategoryRepository.Setup(repo => repo.GetCategoriesWithPost(It.IsAny<SearchCategoryOptionsEnum>())).ReturnsAsync(categoriesWithPost);

            return mockCategoryRepository;
        }
        public static Mock<IPostRepository> GetPostRepository()
        {
            var posts = GetPosts();
            var mockpostRepository = new Mock<IPostRepository>();
            mockpostRepository.Setup(repo => repo.GetAllItems()).ReturnsAsync(posts);
            mockpostRepository.Setup(repo => repo.GetByIdItem(It.IsAny<int>())).ReturnsAsync(
                (int id) =>
                {
                    var pos = posts.FirstOrDefault(c => c.PostId == id);
                    return pos;
                }
            );
            mockpostRepository.Setup(repo => repo.AddItem(It.IsAny<Post>())).ReturnsAsync(
                (Post post) =>
                {
                    posts.Add(post);
                    return post;
                }
            );

            mockpostRepository.Setup(repo => repo.DeleteItem(It.IsAny<Post>())).Callback
                <Post>((entity) => posts.Remove(entity));
            
            mockpostRepository.Setup(repo => repo.UpdateItem(It.IsAny<Post>())).Callback
                <Post>((entity) => { posts.Remove(entity); posts.Add(entity); });

            mockpostRepository.Setup(repo => repo.IsNameAndAuthorAlreadyExist
                (It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string title, string author) =>
                {
                    var matches = posts.
                    Any(a => a.Title.Equals(title) && a.Author.Equals(author));
                    return matches;
                });

            return mockpostRepository;

        }
        public static Mock<IWebinaryRepository> GetWebinarRepository() 
        {
            var webinars = GetWebinars();
            var mockWebinarRepository = new Mock<IWebinaryRepository>();
            mockWebinarRepository.Setup(repo => repo.GetAllItems()).ReturnsAsync(webinars);

            mockWebinarRepository.Setup(repo => repo.GetByIdItem(It.IsAny<int>())).ReturnsAsync(
            (int id) =>
            {
                var pos = webinars.FirstOrDefault(c => c.WebinarId == id);
                return pos;
            });

            mockWebinarRepository.Setup(repo => repo.AddItem(It.IsAny<Webinar>())).ReturnsAsync(
            (Webinar webinar) =>
            {
                webinars.Add(webinar);
                return webinar;
            });

            mockWebinarRepository.Setup(repo => repo.DeleteItem(It.IsAny<Webinar>())).Callback
                <Webinar>((entity) => webinars.Remove(entity));

            mockWebinarRepository.Setup(repo => repo.UpdateItem(It.IsAny<Webinar>())).Callback
                <Webinar>((entity) => { webinars.Remove(entity); webinars.Add(entity); });

            mockWebinarRepository.Setup(repo => repo.GetPagedWebinarsForDate
            (It.IsAny<SearchOptionsWebinarsEnum>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((DateTime date, int page, int pageSize) =>
            {
                var matches = webinars.Where(x => x.Date.Month == date.Month && x.Date.Year == date.Year)
                .Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return matches;
            });

            mockWebinarRepository.Setup(repo => repo.GetTotalCountOfWebinarsForDate
            (It.IsAny<SearchOptionsWebinarsEnum>(), It.IsAny<DateTime?>()))
            .ReturnsAsync((DateTime date) =>
            {
                var matches = webinars.Count
                (x => x.Date.Month == date.Month && x.Date.Year == date.Year);

                return matches;
            });

            return mockWebinarRepository;
        }
        public static List<Post> GetPosts()
        {
            var cat = GetCategories();

            Post p1 = new Post()
            {
                Author = "Marcin",
                Date = DateTime.Now.AddMonths(-6),
                Decription = "Sterownik syacji paliw",
                ImageUrl = "https://stacjapaliw.net/wp-content/uploads/2019/05/driver-schema-kopia-660x443.png",
                PostId = 2,
                Rate = 4,
                Category = cat[1],
                CategoryId = cat[1].CategoryId,
                Title = "OilDriver schemat",
                Url = "https://gist.github.com/polskiprogramistanet"
            };

            Post p2 = new Post()
            {
                Author = "Gerwazy",
                Date = DateTime.Now.AddDays(-23),
                Decription = "Program stacji paliw Oil System GT",
                ImageUrl = "https://stacjapaliw.net/wp-content/uploads/2019/05/oilSmallPanel-kopia-1024x445.png",
                PostId = 3,
                Rate = 9,
                Category = cat[2],
                CategoryId = cat[2].CategoryId,
                Title = "Oil System GT",
                Url = "https://stacjapaliw.net/dla-insert-gt/"
            };
            Post p3 = new Post()
            {
                Author = "Janusz",
                Date = DateTime.Now.AddDays(-34),
                Decription = "System lojalnościowy, karty stałego klienta",
                ImageUrl = "",
                PostId = 4,
                Rate = 7,
                Category = cat[3],
                CategoryId = cat[3].CategoryId,
                Title = "KOntrahent 2.0",
                Url = "https://stacjapaliw.net/"
            };
            Post p4 = new Post()
            {
                Author = "Stefan",
                Date = DateTime.Now.AddYears(-2),
                Decription = "System stacji paliw Wapro",
                ImageUrl = "https://stacjapaliw.net/wp-content/uploads/2022/02/screen3-660x371.png",
                PostId = 5,
                Rate = 6,
                Category = cat[4],
                CategoryId = cat[4].CategoryId,
                Title = "Wapro gas station",
                Url = "https://stacjapaliw.net/dla-wapro/"
            };
            List<Post> p = new List<Post>();
            p.Add(p1); p.Add(p2); p.Add(p3); p.Add(p4);
            return p;
        }
        public static List<Webinar> GetWebinars()
        {
            List<Webinar> w = new List<Webinar>();

            var w1 = new Webinar()
            {
                Title = "Aplikacja C# od Zera Architektura, CQRS, Dobre praktyki",
                AlreadyHappend = false,
                Date = DateTime.Now.AddDays(10),
                Description = @"Ustalenie architektury nie jest prostym zadaniem. Każda decyzja może mieć wielkie komplikacje potem.",
                FacebookEventUrl = "https://www.facebook.com/events/407358067213893/",
                WebinarId = 1,
                ImageUrl = "https://cezarywalenciuk.pl/posts/fileswebinars/17_apliacjacsharpodzeraarchitekturacqrs.jpg",
                SlidesUrl = "",
                WatchFacebookLink = "",
                WatchYoutubeLink = "",
            };
            w.Add(w1);

            var w2 = new Webinar()
            {
                Title = "Kubernetes i Docker : Wytłumacz mi i pokaż",
                AlreadyHappend = false,
                Date = DateTime.Now.AddDays(-40),
                Description = @"Kontenery są tutaj. Kubernetes jest de facto platformą do ich uruchamiania i zarządzania.",
                FacebookEventUrl = "https://www.facebook.com/events/407358067213893/",
                WebinarId = 2,
                ImageUrl = "https://cezarywalenciuk.pl/posts/fileswebinars/17_apliacjacsharpodzeraarchitekturacqrs.jpg",
                SlidesUrl = "https://panniebieski.github.io/webinar-Kubernetes-Docker-Wytlumacz-mi-i-pokaz/",
                WatchFacebookLink = "https://www.facebook.com/watch/live/?v=2775230679405348&ref=watch_permalink",
                WatchYoutubeLink = "https://www.youtube.com/watch?v=7g00wOg9Jto",
            };
            w.Add(w2);


            var w3 = new Webinar()
            {
                Title = "C# 9, Rekordy i duże zmiany w .NET 5",
                AlreadyHappend = false,
                Date = DateTime.Now.AddDays(-60),
                Description = @"Jak utworzyć projekt w .NET 5?",
                FacebookEventUrl = "https://www.facebook.com/events/407358067213893/",
                WebinarId = 3,
                ImageUrl = "https://cezarywalenciuk.pl/posts/fileswebinars/15_csharpirekordy.jpg",
                SlidesUrl = "https://panniebieski.github.io/webinar_CSharp9-Rekordy-i-duze-zamiany-w-net-5/",
                WatchFacebookLink = "https://www.facebook.com/watch/live/?v=2835303250091399&ref=watch_permalink",
                WatchYoutubeLink = "https://www.youtube.com/watch?v=ATbLEyd_1Kg",
            };
            w.Add(w3);

            var w4 = new Webinar()
            {
                Title = "Szybki Trening Sql Server 2",
                AlreadyHappend = false,
                Date = DateTime.Now.AddDays(-70),
                Description = @"Czasami jedyne czego potrzebujemy to dobrego przykładu.",
                FacebookEventUrl = "https://www.facebook.com/events/407358067213893/",
                WebinarId = 4,
                ImageUrl = "https://cezarywalenciuk.pl/posts/fileswebinars/15_csharpirekordy.jpg",
                SlidesUrl = "https://panniebieski.github.io/webinar_CSharp9-Rekordy-i-duze-zamiany-w-net-5/",
                WatchFacebookLink = "https://www.facebook.com/watch/live/?v=2835303250091399&ref=watch_permalink",
                WatchYoutubeLink = "https://www.youtube.com/watch?v=ATbLEyd_1Kg",
            };
            w.Add(w4);

            var w5 = new Webinar()
            {
                Title = "Pytania rekrutacyjne czyli dalsza kariera",
                AlreadyHappend = false,
                Date = DateTime.Now.AddDays(-90),
                Description = @"Jak wygląda szukanie pracy jako programista w 2020 roku? Czy jest lepiej, czy jest gorzej?",
                FacebookEventUrl = "https://www.facebook.com/events/407358067213893/",
                WebinarId = 4,
                ImageUrl = "https://cezarywalenciuk.pl/posts/fileswebinars/15_csharpirekordy.jpg",
                SlidesUrl = "https://panniebieski.github.io/webinar_CSharp9-Rekordy-i-duze-zamiany-w-net-5/",
                WatchFacebookLink = "https://www.facebook.com/watch/live/?v=2835303250091399&ref=watch_permalink",
                WatchYoutubeLink = "https://www.youtube.com/watch?v=ATbLEyd_1Kg",
            };
            w.Add(w5);

            return w;
        }
        private static List<Category> GetCategories()
        {
            Category c1 = new Category()
            {
                CategoryId = 1,
                Name = "CSharp",
                DisplayName = "C#"
            };

            Category c2 = new Category()
            {
                CategoryId = 2,
                Name = "aspnet",
                DisplayName = "ASP.NET"
            };

            Category c3 = new Category()
            {
                CategoryId = 3,
                Name = "triki-z-windows",
                DisplayName = "Triki z Windows"
            };

            Category c4 = new Category()
            {
                CategoryId = 4,
                Name = "docker",
                DisplayName = "Docker"
            };

            Category c5 = new Category()
            {
                CategoryId = 5,
                Name = "filozofia",
                DisplayName = "Filozofia"
            };

            List<Category> p = new List<Category>();
            p.Add(c1); p.Add(c3);
            p.Add(c2); p.Add(c4);
            p.Add(c5);

            return p;
        }
        private static List<Category> GetCategoriesWithPosts()
        {
            var posts = GetPosts();

            Category c1 = new Category()
            {
                CategoryId = 1,
                Name = "CSharp",
                DisplayName = "C#",
                Posts = posts.Where(p => p.CategoryId == 1).ToList()
            };

            Category c2 = new Category()
            {
                CategoryId = 2,
                Name = "aspnet",
                DisplayName = "ASP.NET",
                Posts = posts.Where(p => p.CategoryId == 2).ToList()
            };

            Category c3 = new Category()
            {
                CategoryId = 3,
                Name = "triki-z-windows",
                DisplayName = "Triki z Windows",
                Posts = posts.Where(p => p.CategoryId == 3).ToList()
            };

            Category c4 = new Category()
            {
                CategoryId = 4,
                Name = "docker",
                DisplayName = "Docker",
                Posts = posts.Where(p => p.CategoryId == 4).ToList()
            };

            Category c5 = new Category()
            {
                CategoryId = 5,
                Name = "filozofia",
                DisplayName = "Filozofia",
                Posts = posts.Where(p => p.CategoryId == 5).ToList()
            };


            List<Category> p = new List<Category>();
            p.Add(c1); p.Add(c3);
            p.Add(c2); p.Add(c4);
            p.Add(c5);

            return p;

        }
    }
}
