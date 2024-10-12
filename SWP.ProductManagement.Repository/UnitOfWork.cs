using Microsoft.EntityFrameworkCore;
using SWP.ProductManagement.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.ProductManagement.Repository
{
    public class UnitOfWork : IDisposable
    {
        private ProductManagementContext _context;
        private GenericRepository<Category> _categories;
        private GenericRepository<Product> _productes;

       

        public UnitOfWork(ProductManagementContext context)
        {  _context = context; }
        public GenericRepository<Category> Categories
        { get {
                if (this._categories == null)
                {
                    this._categories = new GenericRepository<Category>(_context);
                }
                return _categories; }
        }
        public GenericRepository<Product> Products
        { get
            {
                if (this._productes == null)
                {
                    this._productes = new GenericRepository<Product>(_context);
                }
                return _productes;
            }
        }
        public async Task SaveAsync()

        {
            await _context.SaveChangesAsync();
        } 
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
    } 
}
