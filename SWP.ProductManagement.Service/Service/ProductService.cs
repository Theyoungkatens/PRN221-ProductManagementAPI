using Microsoft.Identity.Client;
using SWP.ProductManagement.Repository;
using SWP.ProductManagement.Repository.Models;
using SWP.ProductManagement.Service.BusinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SWP.ProductManagement.Service.Service
{
    public class ProductService
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAsync();
            return products.Select(product => new ProductModel
                {
                ProductId = product.ProductId,
                  ProductName = product.ProductName,
                    UnitsInStock = product.UnitsInStock,
                    UnitPrice = product.UnitPrice,
                    CategoryId = product.CategoryId

            });

        }
        public async Task<ProductModel> getProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return null;
            return new ProductModel { ProductId = product.ProductId, 
                ProductName = product.ProductName, 
                UnitsInStock = product.UnitsInStock,
                UnitPrice = product.UnitPrice,
                CategoryId = product.CategoryId,
            };


        }

        public async Task<bool> UpdateProductAsync(int id, ProductModel productModel)
        {
            var productToUpdate = await _unitOfWork.Products.GetByIdAsync(id);
            if (productToUpdate == null) return false;
            productToUpdate.ProductName = productModel.ProductName;
            productToUpdate.UnitsInStock = productModel.UnitsInStock;
            productToUpdate.UnitPrice = productModel.UnitPrice;
            productToUpdate.CategoryId = productModel.CategoryId;

            _unitOfWork.Products.Update(productToUpdate);
            await _unitOfWork.SaveAsync();
            return true;

        }
        public async Task<int> InsertProductAsync(ProductModel productModel)
        {
            // Create an instance of Product, not ProductModel
            var productEntity = new Product
            {
                ProductName = productModel.ProductName,
                UnitsInStock = productModel.UnitsInStock,
                UnitPrice = productModel.UnitPrice,
                CategoryId = productModel.CategoryId
            };

            await _unitOfWork.Products.InsertAsync(productEntity);
            await _unitOfWork.SaveAsync();

            return productEntity.ProductId; // This assumes that ProductId is auto-generated
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null) return false;

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.SaveAsync();
            return true;

        }
        public async Task<bool> ProductExistAsync(int id)
        {
            return await _unitOfWork.Products.IsExist(id);
        }
    } }
