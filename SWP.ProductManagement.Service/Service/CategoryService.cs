﻿using SWP.ProductManagement.Repository;
using SWP.ProductManagement.Repository.Models;
using SWP.ProductManagement.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.ProductManagement.Service.Service
{
    public class CategoryService
    {
        private readonly UnitOfWork _unitOfWork;
        public CategoryService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<CategoryModel>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAsync(includeProperties: "Products");
            return categories.Select(category => new CategoryModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Products = category.Products.Select(product => new ProductModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitsInStock = product.UnitsInStock,  // Ensure this is properly mapped
                    UnitPrice = product.UnitPrice,        // Ensure this is properly mapped
                    CategoryId = product.CategoryId
                }).ToList()
            });
        }


        public async Task<CategoryModel> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(
                filter: category => category.CategoryId == id,
                includeProperties: "Products"
            );

            var categoryEntity = category.FirstOrDefault();
            if (categoryEntity == null) return null;

            return new CategoryModel
            {
                CategoryId = categoryEntity.CategoryId,
                CategoryName = categoryEntity.CategoryName,
                Products = categoryEntity.Products.Select(product => new ProductModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    UnitsInStock = product.UnitsInStock ?? 0, // Safe null handling
                    UnitPrice = product.UnitPrice ?? 0, // Safe null handling
                    CategoryId = product.CategoryId
                }).ToList()
            };
        }
        public async Task<int> InsertCategoryAsync(CategoryModel categoryModel)
        {
            var categoryEntity = new Category
            {
                CategoryName = categoryModel.CategoryName
            };

            await _unitOfWork.Categories.InsertAsync(categoryEntity);
            await _unitOfWork.SaveAsync();
            return categoryEntity.CategoryId;
        }
        public async Task<bool> UpdateCategoryAsync(int id, CategoryModel categoryModel)
        {
            var categoryToUpdate = await _unitOfWork.Categories.GetByIdAsync(id);
            if (categoryToUpdate == null) return false;

            categoryToUpdate.CategoryName = categoryModel.CategoryName;
            _unitOfWork.Categories.Update(categoryToUpdate);
            await _unitOfWork.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) return false;
            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveAsync();
            return true;

        }
        public async Task<bool> CategoryExistAsync(int id)
        {
            return await _unitOfWork.Categories.IsExist(id);
        }
        public async Task<IEnumerable<ProductModel>> GetProductsByCategoryIdAsync(int id)
        {
            var products = await _unitOfWork.Products.GetAsync(p => p.CategoryId == id);
            return products.Select(product => new ProductModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitsInStock = product.UnitsInStock,
                UnitPrice = product.UnitPrice,
                CategoryId = product.CategoryId,
            });
        }

    }
}
