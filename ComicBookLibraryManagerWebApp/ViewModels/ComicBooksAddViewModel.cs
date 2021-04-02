﻿using ComicBookShared.Data;
using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComicBookLibraryManagerWebApp.ViewModels
{
    /// <summary>
    /// View model for the "Add Comic Book" view.
    /// </summary>
    public class ComicBooksAddViewModel
        : ComicBooksBaseViewModel
    {
        [Display(Name = "Artist")]
        public int ArtistId { get; set; }
        [Display(Name = "Role")]
        public int RoleId { get; set; }

        public SelectList ArtistSelectListItems { get; set; }
        public SelectList RoleSelectListItems { get; set; }

        public ComicBooksAddViewModel()
        {
            // Set the comic book default values.
            ComicBook.IssueNumber = 1;
            ComicBook.PublishedOn = DateTime.Today;
        }

        /// <summary>
        /// Initializes the view model.
        /// </summary>
        public override void Init(Repository repository,
            SeriesRepository seriesRepository,
            ArtistsRepository artistsRepository)
        {
            base.Init(repository, seriesRepository, artistsRepository);

            ArtistSelectListItems = new SelectList(
                artistsRepository.GetList(),
                "Id", "Name");
            RoleSelectListItems = new SelectList(
                repository.GetRoles(),
                "Id", "Name");
        }
    }
}