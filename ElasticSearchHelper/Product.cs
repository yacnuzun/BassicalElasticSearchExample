using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchHelper
{
    public class Product:BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
    public class UpdateProductDTO:BaseDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class BaseDTO:IBaseDto
    {
        public string Id { get; set; }
    }
    public class BaseModel:IBaseModel
    {
        public string Id { get; set; }
    }
    public interface IBaseModel
    {

    }
    public interface IBaseDto
    {

    }
}
