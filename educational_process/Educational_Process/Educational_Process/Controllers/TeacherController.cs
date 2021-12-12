﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Educational_Process.Domain;
using Educational_Process.Models;
using Educational_Process.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Educational_Process.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherServices;
        private readonly ISubjectService _subjectServices;
        private readonly EducationalProcessContext _educationalProcessContext;

        public TeacherController(
            ITeacherService teacherServices,
            ISubjectService subjectServices,
            EducationalProcessContext educationalProcessContext)
        {
            _subjectServices = subjectServices;
            _teacherServices = teacherServices;
            _educationalProcessContext = educationalProcessContext;

        }
        // GET: ProductController/Index
        public IActionResult Index(string SearchString)
        {
            var model = _teacherServices.GetAll();
            if (!string.IsNullOrEmpty(SearchString))
            {
                model = model.Where(x => x.FirstName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)
                || x.SecondName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)
                || x.ThirdName.Contains(SearchString, StringComparison.InvariantCultureIgnoreCase)).ToList();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            var model = _teacherServices.GetById(id);
            var subjects = _subjectServices.GetAll();
            ViewBag.Subjects = new SelectList(subjects, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public IActionResult Add(Teacher model)
        {
            _teacherServices.Create(model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            _teacherServices.Delete(id);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _teacherServices.GetById(id);

            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult Edit(Teacher model)
        {
            _teacherServices.Edit(model.Id, model);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _teacherServices.GetById(id);

            TeacherViewModel teacherViewModel = new TeacherViewModel()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                SubjectName = string.Join(", ", model.Subjects.ToList())
            };

            return View("Details", teacherViewModel);
        }
    }
}
