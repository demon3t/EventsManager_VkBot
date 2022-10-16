using System;
using System.Collections.Generic;
using System.Text;

namespace vkBot.Basic
{
    public class Signed
    {
        #region свойства

        public int EventId
        {
            get { return _eventId; }
            set
            {
                _eventId = value;
            }
        }
        private int _eventId;

        public long ClientId
        {
            get { return _clientId; }
            set
            {
                _clientId = value;
            }
        }
        private long _clientId;

        public bool Sign
        {
            get { return _sign; }
            set
            {
                _sign = value;
            }
        }
        private bool _sign;

        #endregion

        public Signed(int eventId, long clientId)
        {
            _eventId = eventId;
            _clientId = clientId;
            _sign = false;
        }
    }
}
