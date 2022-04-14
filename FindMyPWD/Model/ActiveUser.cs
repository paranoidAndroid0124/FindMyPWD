namespace FindMyPWD.Model
{
    public class ActiveUser
    {
        private string _caregiver_id { get; set; }
        private string _plwd_id { get; set; }
        private string _watch_id { get; set; }

        public ActiveUser(string caregiver_id, string plwd_id, string watch_id)
        {
            _caregiver_id = caregiver_id;
            _plwd_id = plwd_id;
            _watch_id = watch_id;
        }

        public string getActiveCaregiverID()
        {
            return _caregiver_id;
        }
        public void setActiveCaregiverID(string id)
        {
             _caregiver_id = id;
        }

        public string getActivePlwdID()
        {
            return _plwd_id;
        }

        public void setActivePlwdID(string id)
        {
            _plwd_id = id;
        }

        public string getActiveWatchID()
        {
            return _watch_id;
        }

        public void setActiveWatchID(string id)
        {
            _watch_id = id;
        }
    }
}
