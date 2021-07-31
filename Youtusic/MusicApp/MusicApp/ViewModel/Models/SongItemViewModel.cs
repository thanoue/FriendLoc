using System;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MusicApp.ViewModel
{
    public class SongItemViewModel : BaseViewModel
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        private SongTypes _type;
        public SongTypes Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        
        private string _authorName;
        public string AuthorName
        {
            get => _authorName;
            set
            {
                _authorName = value;
                OnPropertyChanged(nameof(AuthorName));
            }
        }

        private string _url;
        public string Url
        {
            get => _url;
            set
            {
                _url = value;
                OnPropertyChanged(nameof(Url));
            }
        }

        private string _id;
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private double _duration;
        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        private string _authorId;
        public string AuthorId
        {
            get => _authorId;
            set
            {
                _authorId = value;
                OnPropertyChanged(nameof(AuthorId));
            }
        }

        private DateTime _publishedAt;
        public DateTime PublishedAt
        {
            get => _publishedAt;
            set
            {
                _publishedAt = value;
                OnPropertyChanged(nameof(PublishedAt));
            }
        }  
        
        private string _publishedTimeStr;
        public string PublishedTimeStr 
        {
            get => _publishedTimeStr;
            set
            {
                _publishedTimeStr = value;
                OnPropertyChanged(nameof(PublishedTimeStr));
            }
        }

        private string _smallThumbnailUrl;
        public string SmallThumbnailUrl
        {
            get => _smallThumbnailUrl;
            set
            {
                _smallThumbnailUrl = value;
                OnPropertyChanged(nameof(SmallThumbnailUrl));
            }
        }

        private string _bigThumbnailUrl;
        public string BigThumbnailUrl
        {
            get => _bigThumbnailUrl;
            set
            {
                _bigThumbnailUrl = value;
                OnPropertyChanged(nameof(BigThumbnailUrl));
            }
        }
        
        private int _audioBitRate; 
        public int AudioBitRate
        {
            get => _audioBitRate;
            set
            {
                _audioBitRate = value;
                OnPropertyChanged(nameof(AudioBitRate));
            }
        }

        private long _fileSize;
        public long FileSize
        {
            get => _fileSize;
            set
            {
                _fileSize = value;
                OnPropertyChanged(nameof(FileSize));
            }
        }
        
        [JsonIgnore]
        public  Action<string> OnPlay { get; set; }

        [JsonIgnore]
        public  ICommand PlayCommand { get; set; }

        public SongItemViewModel()
        {
            PlayCommand = new Command(() =>
            {
                OnPlay?.Invoke(this.Id);
            }, () => true );
        }
    }

    public enum SongTypes
    {
        None,
        Online,
        Offline
    }
}