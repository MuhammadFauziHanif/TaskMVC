using Microsoft.AspNetCore.Mvc;
using MyWebFormApp.BLL;
using MyWebFormApp.BLL.DTOs;
using MyWebFormApp.BLL.Interfaces;
using MyWebFormApp.DAL;

namespace SampleMVC.Controllers;

public class ArticlesController : Controller
{
    private readonly IArticleBLL _articleBLL;
    private readonly ICategoryBLL _categoryBLL;

    public ArticlesController(IArticleBLL articleBLL, ICategoryBLL categoryBLL)
    {
        _articleBLL = articleBLL;
        _categoryBLL = categoryBLL;
    }

    public IActionResult Index()
    {
        if (TempData["message"] != null)
        {
            ViewData["message"] = TempData["message"];
        }

        var categories = _categoryBLL.GetAll();
        ViewBag.Categories = categories;

        var models = _articleBLL.GetArticleByCategory(1);
        return View(models);
    }

    public IActionResult Detail(int id)
    {
        var model = _articleBLL.GetArticleById(id);
        return View(model);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(ArticleCreateDTO articleCreate)
    {
        try
        {
            _articleBLL.Insert(articleCreate);
            //ViewData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Article Success !</div>";
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Add Data Article Success !</div>";
        }
        catch (Exception ex)
        {
            //ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var model = _articleBLL.GetArticleById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(int id, ArticleUpdateDTO articleEdit)
    {
        try
        {
            _articleBLL.Update(articleEdit);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Edit Data Article Success !</div>";
        }
        catch (Exception ex)
        {
            ViewData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(articleEdit);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Search(string search)
    {
        ViewData["search"] = search;

        //var models = _articleBLL.GetByName(search);
        //return View("Index", models);
        return View("Index");
    }

    public IActionResult Delete(int id)
    {
        var model = _articleBLL.GetArticleById(id);
        if (model == null)
        {
            TempData["message"] = @"<div class='alert alert-danger'><strong>Error!</strong>Article Not Found !</div>";
            return RedirectToAction("Index");
        }
        return View(model);
    }

    [HttpPost]
    public IActionResult Delete(int id, ArticleDTO article)
    {
        try
        {
            _articleBLL.Delete(id);
            TempData["message"] = @"<div class='alert alert-success'><strong>Success!</strong>Delete Data Article Success !</div>";
        }
        catch (Exception ex)
        {
            TempData["message"] = $"<div class='alert alert-danger'><strong>Error!</strong>{ex.Message}</div>";
            return View(article);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DisplayDropdownList(string CategoryID)
    {
        ViewBag.CategoryID = CategoryID;
        ViewBag.Message = $"You selected {CategoryID}";

        ViewBag.Categories = _categoryBLL.GetAll();

        return View("Index");
    }


}
