﻿using FluentAssertions;
using MicroStore.Catalog.Application.Models.Categories;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Entities.ElasticSearch;
namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class CategoryAssertionExtensions
    {
        
        public static void AssertCategoryModel(this Category category, CategoryModel model)
        {

            category.Name.Should().Be(model.Name);
            category.Description.Should().Be(model.Description);
        }

        public static void AssertElasticCategory(this ElasticCategory elasticCategory, Category category)
        {
            elasticCategory.Id.Should().Be(category.Id);
            elasticCategory.Name.Should().Be(category.Name);
            elasticCategory.Description.Should().Be(category.Description);
        }
    }
}