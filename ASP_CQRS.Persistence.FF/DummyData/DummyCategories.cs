﻿using System;
using System.Collections.Generic;
using System.Text;
using ASP_CQRS.Domain.Entities;

namespace ASP_CQRS.Persistence.FF.DummyData
{
    public class DummyCategories
    {
        public static List<Category> Get()
        {
            Category c1 = new Category()
            {
                CategoryId = DummySeed.Csharp,
                Name = "CSharp",
                DisplayName = "C#"
            };

            Category c2 = new Category()
            {
                CategoryId = DummySeed.Aspnet,
                Name = "aspnet",
                DisplayName = "ASP.NET"
            };

            Category c3 = new Category()
            {
                CategoryId = DummySeed.TrickiZWindows,
                Name = "triki-z-windows",
                DisplayName = "Triki z Windows"
            };

            Category c4 = new Category()
            {
                CategoryId = DummySeed.Docker,
                Name = "docker",
                DisplayName = "Docker"
            };

            Category c5 = new Category()
            {
                CategoryId = DummySeed.Filozofia,
                Name = "filozofia",
                DisplayName = "Filozofia"
            };


            List<Category> p = new List<Category>();
            p.Add(c1); p.Add(c3);
            p.Add(c2); p.Add(c4);
            p.Add(c5);

            return p;
        }
    }
}
