﻿using FluentAssertions;
using MicroStore.Catalog.Application.Abstractions.Products;
using MicroStore.Catalog.Application.Abstractions.ProductTags;
using MicroStore.Catalog.Application.Abstractions.SpecificationAttributes;
using MicroStore.Catalog.Domain.Entities;
using MicroStore.Catalog.Domain.ValueObjects;
using MicroStore.Catalog.Entities.ElasticSearch;
using MicroStore.Catalog.Entities.ElasticSearch.Common;
using System;

namespace MicroStore.Catalog.Application.Tests.Extensions
{
    public static class ProductAssertionExtensions
    {
        public static void AssertProductModel(this Product product, ProductModel model)
        {
            product.Sku.Should().Be(model.Sku);
            product.Name.Should().Be(model.Name);
            product.ShortDescription.Should().Be(model.ShortDescription);
            product.LongDescription.Should().Be(model.LongDescription);
            product.Price.Should().Be(model.Price);
            product.OldPrice.Should().Be(model.OldPrice);
            product.Weight.Should().Be(model.Weight.AsWeight());
            product.Dimensions.Should().Be(model.Dimensions.AsDimension());
            product.IsFeatured.Should().Be(model.IsFeatured);

            product.Categories.OrderBy(x => x.Id).Should().Equal(model.Categories?.OrderBy(x => x), (left, right) =>
            {
                return left.Id == right;
            });

            product.Manufacturers.OrderBy(x => x.Id).Should().Equal(model.Manufacturers?.OrderBy(x => x), (left, right) =>
            {
                return left.Id == right;
            });

            product.Tags.OrderBy(x => x.Name).Should().Equal(model.ProductTags?.OrderBy(x => x), (left, right) =>
            {
                return left.Name == right;
            });

            product.SpecificationAttributes.OrderBy(x => x.AttributeId).Should().Equal(model.SpecificationAttributes?.OrderBy(x => x.AttributeId), (left, right) =>
            {
                return left.AttributeId == right.AttributeId &&
                    left.OptionId == right.OptionId;
            });

        }

        public static void AssertElasticProduct(this ElasticProduct elasticProduct, Product product)
        {
            elasticProduct.Id.Should().Be(product.Id);

            elasticProduct.Name.Should().Be(product.Name);

            elasticProduct.Sku.Should().Be(product.Sku);

            elasticProduct.ShortDescription.Should().Be(product.ShortDescription);

            elasticProduct.LongDescription.Should().Be(product.LongDescription);

            elasticProduct.Price.Should().Be(product.Price);

            elasticProduct.OldPrice.Should().Be(product.OldPrice);

            elasticProduct.Weight?.AssertElasticWeight(product.Weight);

            elasticProduct.Dimensions?.AssertElasticDimension(product.Dimensions);

            elasticProduct.IsFeatured.Should().Be(product.IsFeatured);

            elasticProduct.Categories.OrderBy(x => x.Id)
                .Zip(product.Categories.OrderBy(x => x.Id))
                .ToList()
                .ForEach(categoryTuple => categoryTuple.First.AssertElasticProductCategory(categoryTuple.Second));


            elasticProduct.Manufacturers.OrderBy(x => x.Id)
                .Zip(product.Manufacturers.OrderBy(x => x.Id))
                .ToList()
                .ForEach(manufacturerTuple =>  manufacturerTuple.First.AssertElasticProductManufacturer(manufacturerTuple.Second));


            elasticProduct.Tags.OrderBy(x => x.Id)
                .Zip(product.Tags.OrderBy(x => x.Id))
                .ToList()
                .ForEach(productTagTuple => productTagTuple.First.AssertElasticProductTag(productTagTuple.Second));


            elasticProduct.SpecificationAttributes.OrderBy(x => x.Id)
                .Zip(product.SpecificationAttributes.OrderBy(x => x.Id))
                .ToList()
                .ForEach(specificationAttributeTuble => specificationAttributeTuble.First.AssertElasticProductSpecificationAttribute(specificationAttributeTuble.Second));

        }


        public static void AssertElasticWeight(this ElasticWeight elasticWeight, Weight model)
        {
            elasticWeight.Value.Should().Be(model.Value);

            elasticWeight.Unit.Should().Be(model.Unit);
        }

        public static void AssertElasticDimension(this ElasticDimension elasticDimension, Dimension model)
        {
            elasticDimension.Length.Should().Be(model.Length);

            elasticDimension.Width.Should().Be(model.Width);

            elasticDimension.Height.Should().Be(model.Height);

            elasticDimension.Unit.Should().Be(model.Unit);
        }


        public static void AssertProductTagModel(this Tag productTag, ProductTagModel model)
        {
            productTag.Name.Should().Be(model.Name);
            productTag.Description.Should().Be(model.Description);
        }

        public static void AssertElasticProductTag(this ElasticTag elasticProductTag, Tag productTag)
        {
            elasticProductTag.Id.Should().Be(productTag.Id);
            elasticProductTag.Name.Should().Be(productTag.Name);
            elasticProductTag.Description.Should().Be(productTag.Description);
        }

        public static void AssertSpecificationAttributeModel(this SpecificationAttribute attribute, SpecificationAttributeModel model)
        {
            attribute.Name.Should().Be(model.Name);
            attribute.Description.Should().Be(model.Description);

            foreach (var tuple in attribute.Options.OrderBy(x => x.Name).Zip(model.Options!.OrderBy(x => x.Name).ToList()))
            {
                tuple.First.AssertSpecificationAttributeOptionModel(tuple.Second);
            }

        }


        public static void AssertElasticSpecificationAttribute(this ElasticSpecificationAttribute elasticSpcificationAttribute, SpecificationAttribute specificationAttribute)
        {
            elasticSpcificationAttribute.Id.Should().Be(elasticSpcificationAttribute.Id);
            elasticSpcificationAttribute.Name.Should().Be(specificationAttribute.Name);
            elasticSpcificationAttribute.Description.Should().Be(specificationAttribute.Description);


            elasticSpcificationAttribute.Options.OrderBy(x => x.Id)
                .Zip(specificationAttribute.Options.OrderBy(x => x.Id))
            .ToList()
                .ForEach(tuple => tuple.First.AssertElasticSpecificationAttributeOptionModel(tuple.Second));

        }

        public static void AssertSpecificationAttributeOptionModel(this SpecificationAttributeOption option, SpecificationAttributeOptionModel model)
        {
            option.Name.Should().Be(model.Name);
        }

        public static void AssertElasticSpecificationAttributeOptionModel(this ElasticSpecificationAttributeOption elasticOption, SpecificationAttributeOption option)
        {
            elasticOption.Id.Should().Be(option.Id);
            elasticOption.Value.Should().Be(option.Name);
        }


        public static void AssertElasticProductSpecificationAttribute(this ElasticProductSpecificationAttribute elasticProductSpecificationAttribute, ProductSpecificationAttribute productSpecificationAttribute)
        {
            elasticProductSpecificationAttribute.Id.Should().Be(productSpecificationAttribute.Id);
            elasticProductSpecificationAttribute.AttributeId.Should().Be(productSpecificationAttribute.AttributeId);
            elasticProductSpecificationAttribute.OptionId.Should().Be(productSpecificationAttribute.OptionId);
            elasticProductSpecificationAttribute.Name.Should().Be(productSpecificationAttribute.Attribute.Name);
            elasticProductSpecificationAttribute.Value.Should().Be(productSpecificationAttribute.Option.Name);
        }
    }
}
