namespace ElectionWeb.Models.ViewModels
{
    public class ResultUploadViewModel
    {
        public IFormFile File { get; set; }
    }

    public class ResultViewModel
    {
        public long Id { get; set; }
        public string PollingUnitName { get; set; }
        public long PollingUnitId { get; set; }
        public string WardName { get; set; }
        public long WardId { get; set; }
        public long TotalVoteCount { get; set; }
        public long TotalParties { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
    }

    public class StateResultViewModel : ResultViewModel
    {
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class WardResultViewModel
    {
        public long Id { get; set; }
        public string Ward { get; set; }
        public string LgaName { get; set; }
        public long LgaId { get; set; }
        public long WardId { get; set; }
        public long PollingUnitCounts { get; set; } 
        public long TotalVote { get; set; }
        public long TotalParties { get; set; }
        public List<PartyResultViewModel> PartyResult { get; set; }
    }


    public class ResultDetailViewModel
    {
        public long Id { get; set; }
        public string PollingUnitName { get; set; }
        public long PollingUnitId { get; set; }
        public string WardName { get; set; }
        public long WardId { get; set; }
        public long TotalVoteCount { get; set; }
        public long TotalParties { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public List<PartyResultViewModel> PartyResult { get; set; }
    }

    public class PartyResultViewModel
    {
        public long Id { get; set; }
        public string PartyName { get; set; }
        public long PartyCount { get; set; }
    }
}
