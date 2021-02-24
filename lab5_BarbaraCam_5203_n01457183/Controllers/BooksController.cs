using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace lab5_BarbaraCam_5203_n01457183.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            IList<Models.Book> bookList = new List<Models.Book>();

            //load book.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                doc.Load(path);
                XmlNodeList books = doc.GetElementsByTagName("book");

                foreach(XmlElement b in books)
                {
                    Models.Book book = new Models.Book();
                    
                    book.FirstName = b.GetElementsByTagName("firstname")[0].InnerText;
                    book.LastName = b.GetElementsByTagName("lastname")[0].InnerText;
                    //book.MiddleName = b.GetElementsByTagName("middlename")[0].InnerText;
                    book.Id = b.GetElementsByTagName("id")[0].InnerText;
                    book.Title = b.GetElementsByTagName("title")[0].InnerText;

                    bookList.Add(book);

                }

            }
            return View(bookList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var book = new Models.Book();
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(Models.Book b)
        {
            //load book.xml
            string path = Request.PathBase + "App_Data/books.xml";
            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(path))
            {
                //if file exist, just load it and create new person
                doc.Load(path);

                //create a new person
                XmlElement book = _CreateBookElement(doc, b);

                //get the root element
                doc.DocumentElement.AppendChild(book);
            }
            else
            {
                //if file does not exist, create and create new person
                XmlNode dec = doc.CreateXmlDeclaration("1.0", "utf-8", "");
                doc.AppendChild(dec);
                XmlNode root = doc.CreateElement("books");

                //create a new person

                XmlElement book = _CreateBookElement(doc, b);
                root.AppendChild(book);
                doc.AppendChild(root);

            }
            doc.Save(path);

            return View();
        }

        private XmlElement _CreateBookElement(XmlDocument doc, Models.Book newBook)
        {
            XmlElement book = doc.CreateElement("book");

            XmlNode id = doc.CreateElement("id");
            XmlNode title = doc.CreateElement("title");
            id.InnerText = newBook.Id;
            title.InnerText = newBook.Title;


            XmlNode author = doc.CreateElement("author");
            XmlNode first = doc.CreateElement("firstname");
            XmlNode last = doc.CreateElement("lastname");
            first.InnerText = newBook.FirstName;
            last.InnerText = newBook.LastName;

            author.AppendChild(first);
            author.AppendChild(last);

            book.AppendChild(id);
            book.AppendChild(title);
            book.AppendChild(author);


            return book;

        }









    }
}
