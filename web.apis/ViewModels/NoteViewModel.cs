using System.ComponentModel.DataAnnotations;

namespace web.apis.ViewModels
{
    public class SingleNoteBindingModel
    {
        [Required]
        public int Id { get; set; }
    }

    public class NoteBindingModel
    {
        [Required]
        public int IdeaId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class NoteViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IdeaId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }

    public class NoteUpdateBindingModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IdeaId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class NodeIdeaIdBindingModel
    {
        [Required]
        public int Id { get; set; }
    }
}

