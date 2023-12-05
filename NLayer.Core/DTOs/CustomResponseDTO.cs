using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    //tek bir model döncek generic olarak bir T datası alsın.
    public class CustomResponseDTO<T>
    {
        //geriye bir değer dönmek istemediğimiz responselar olabilir bunun için boş bir DTO oluşturacağız.
        public T Data { get; set; } //generic T datasını aldık

        public List<string> Errors { get; set; }

        [JsonIgnore] //status kodunu jsona çevirme, cliente göstermemek için.
        public int StatusCode { get; set; }

        //başarılı durum => geriye data dönen metod
        public static CustomResponseDTO<T> Success(int statusCode, T data)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode, Data = data }; // yeni Dto oluştur aldığın statuscodu ve datayı geri dön hata null
        }

        //başarılı durum =>  geriye data dönmeyen metod
        public static CustomResponseDTO<T> Success(int statusCode)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode };
        }

        //başarısız durum => geriye error listesi ve statusCode dönen metod
        public static CustomResponseDTO<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode, Errors = errors };
        }

        //başarısız durum => geriye error ve statusCode dönen metod
        public static CustomResponseDTO<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDTO<T> { StatusCode = statusCode, Errors = new List<string> { error } }; // yeni Dto oluştur aldığın statuscodu ve datayı geri dön hata null
        }
    }
}