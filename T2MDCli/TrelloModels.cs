﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldenSyrupGames.T2MD
{
    /// <summary>
    /// Things common to multiple Trello models I need to work with.
    /// </summary>
    public interface ITrelloCommon
    {
        public string Name { get; set; }

        /// <summary>
        /// archived
        /// </summary>
        public bool Closed { get; set; }
    }

    /// <summary>
    /// Partial model of the API response to /board (maybe?) to deserialize json to. We only use
    /// this as listed in the response to /members/me/boards.
    /// Pretty sure this is the same as TrelloBoardModel so could combine.
    /// </summary>
    public class TrelloApiBoardModel : ITrelloCommon
    {
        public string Name { get; set; } = "";
        public string ShortLink { get; set; } = "";
        public string ID { get; set; } = "";

        /// <summary>
        /// archived
        /// </summary>
        public bool Closed { get; set; } = false;

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (
                string.IsNullOrEmpty(ID)
                || string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(ShortLink)
            )
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of the json from backing up a Trello board (via the web UI, not via the API.
    /// E.g. trello.com/b/[id].json
    /// </summary>
    public class TrelloBoardModel : ITrelloCommon
    {
        public string Name { get; set; } = "";
        public string ShortLink { get; set; } = "";
        public List<TrelloListModel> Lists { get; set; } = new List<TrelloListModel>();
        public List<TrelloCardModel> Cards { get; set; } = new List<TrelloCardModel>();
        public List<TrelloChecklistModel> Checklists { get; set; } =
            new List<TrelloChecklistModel>();
        public List<TrelloActionModel> Actions { get; set; } = new List<TrelloActionModel>();

        /// <summary>
        /// archived
        /// </summary>
        public bool Closed { get; set; } = false;

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ShortLink))
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of lists forming part of TrelloBoardModel, + some extra properties we store
    /// results in.
    /// </summary>
    public class TrelloListModel : ITrelloCommon
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";

        /// <summary>
        /// the full path on disk to the folder we created for this list
        /// </summary>
        public string FolderPath { get; set; } = "";

        /// <summary>
        /// the full path on disk to the folder we created for archived cards in this list
        /// </summary>
        public string ArchiveFolderPath { get; set; } = "";

        /// <summary>
        /// as we process each non-archived card the number we're up to is recorded here, so we can
        /// have per-list card counts and ordering
        /// </summary>
        public int NonArchivedCardIndex { get; set; } = 0;

        /// <summary>
        /// NonArchivedCardIndex but for archived cards, so they're both numbered without gaps
        /// </summary>
        public int ArchivedCardIndex { get; set; } = 0;

        /// <summary>
        /// archived
        /// </summary>
        public bool Closed { get; set; } = false;

        /// <summary>
        /// list position in board
        /// </summary>
        public double Pos { get; set; } = 0.0;

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ID))
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of cards forming part of TrelloBoardModel
    /// </summary>
    public class TrelloCardModel : ITrelloCommon
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";

        /// <summary>
        /// ID of the list this card is in
        /// </summary>
        public string IDList { get; set; } = "";

        /// <summary>
        /// card description
        /// </summary>
        public string Desc { get; set; } = "";

        /// <summary>
        /// card position in list
        /// </summary>
        public double Pos { get; set; } = 0.0;

        /// <summary>
        /// archived
        /// </summary>
        public bool Closed { get; set; } = false;

        /// <summary>
        /// Contains a useful short ID. <para />
        /// Looks like <c>https://trello.com/c/aaa11bb2</c>. No trailing /.
        /// </summary>
        public string ShortUrl { get; set; } = "";

        /// <summary>
        /// The board this card is in. <para/>
        /// Specified at the start of each card's processing, only safe to use after that.
        /// </summary>
        public TrelloBoardModel? Board { get; set; }

        /// <summary>
        /// the checklists that are in this card
        /// </summary>
        public List<string> IDChecklists { get; set; } = new List<string>();

        public List<TrelloAttachmentModel> Attachments { get; set; } =
            new List<TrelloAttachmentModel>();

        /// <summary>
        /// The path to the main file we wrote the card's description to. Populated after
        /// processing.
        /// </summary>
        public string DescriptionPath { get; set; } = "";

        /// <summary>
        /// The path to the file we wrote the card's comments to. Populated after
        /// processing.
        /// </summary>
        public string CommentsPath { get; set; } = "";

        /// <summary>
        /// The path to the file we wrote the card's checklists to. Populated after
        /// processing.
        /// </summary>
        public string ChecklistsPath { get; set; } = "";

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (
                string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(ID)
                || string.IsNullOrEmpty(IDList)
                || string.IsNullOrEmpty(ShortUrl)
            )
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of checklists forming part of TrelloBoardModel
    /// </summary>
    public class TrelloChecklistModel
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";

        /// <summary>
        /// ID of the card this checklist is in
        /// </summary>
        public string IDCard { get; set; } = "";

        /// <summary>
        /// checklist position in card
        /// </summary>
        public double Pos { get; set; } = 0.0;

        /// <summary>
        /// items in this checklist
        /// </summary>
        public List<TrelloCheckItemModel> CheckItems { get; set; } =
            new List<TrelloCheckItemModel>();

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (
                string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(ID)
                || string.IsNullOrEmpty(IDCard)
            )
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of checklists forming part of TrelloBoardModel
    /// </summary>
    public class TrelloCheckItemModel
    {
        public string Name { get; set; } = "";
        public string ID { get; set; } = "";

        /// <summary>
        /// ID of the checklist this entry is in
        /// </summary>
        public string IDChecklist { get; set; } = "";

        /// <summary>
        /// position in checklist
        /// </summary>
        public double Pos { get; set; } = 0.0;

        /// <summary>
        /// whether this item is checked ("complete") or not ("not complete")
        /// </summary>
        public string State { get; set; } = "";

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (
                string.IsNullOrEmpty(Name)
                || string.IsNullOrEmpty(ID)
                || string.IsNullOrEmpty(State)
            )
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of card actions (like commenting) forming part of TrelloBoardModel
    /// </summary>
    public class TrelloActionModel
    {
        public string ID { get; set; } = "";

        /// <summary>
        /// "commentCard" if it's a card comment
        /// </summary>
        public string Type { get; set; } = "";

        /// <summary>
        /// ISO8601 date
        /// </summary>
        public string Date { get; set; } = "";
        public TrelloActionDataModel Data { get; set; } = new TrelloActionDataModel();

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (
                string.IsNullOrEmpty(ID)
                || string.IsNullOrEmpty(Type)
                || string.IsNullOrEmpty(Date)
                || Data == null
            )
            {
                return false;
            }
            // else
            return true;
        }
    }

    /// <summary>
    /// Partial model of card actions inner data forming part of TrelloBoardModel
    /// </summary>
    public class TrelloActionDataModel
    {
        /// <summary>
        /// E.g. the contents of the comment
        /// </summary>
        public string Text { get; set; } = "";

        /// <summary>
        /// A subset of the full card data. Importantly has the ID of the card this is for
        /// </summary>
        public TrelloCardModel Card { get; set; } = new TrelloCardModel();
    }

    /// <summary>
    /// partial model of a card's attachment forming part of TrelloBoardModel, + some extra
    /// properties we store results in.
    /// </summary>
    public class TrelloAttachmentModel
    {
        public bool IsUpload { get; set; } = false;
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
        public string ID { get; set; } = "";
        public string FileName { get; set; } = "";

        /// <summary>
        /// The path that we save the attachment to if we download it, relative to the description
        /// etc files. <para />
        /// This is so we can download them all in one awaited go, record the relative paths here,
        /// then find+replace later.
        /// </summary>
        public string RelativeAttachmentPathSpacesReplaced { get; set; } = "";

        // workaround for no Required attribute. Returns true if we have data in fields that should
        // always have data
        public bool AreAllRequiredFieldsFilled()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Url) || string.IsNullOrEmpty(ID))
            {
                return false;
            }
            // else
            return true;
        }
    }
}
