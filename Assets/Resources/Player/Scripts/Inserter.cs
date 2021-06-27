using Biosearcher.InputHandling;
using UnityEngine;

namespace Biosearcher.Player
{
    public class Inserter : MonoBehaviour
    {
        #region Properties

        //todo: delete grabber from here
        protected Grabber _grabber;
        
        protected InserterInput _input;
    
        protected IInsertable _insertable;
        protected IInsertFriendly _insertFriendly;

        public IInsertable Insertable
        {
            get => _insertable;
            set => _insertable = value;
        }
        public IInsertFriendly InsertFriendly
        {
            get => _insertFriendly;
            set => _insertFriendly = value;
        }
    
        #endregion

        #region MonoBehaviour methods

        private void Awake()
        {
            _grabber = GetComponent<Grabber>();
            _input = new InserterInput(new Presenter(this));
        }
        protected void OnDestroy() => _input.Dispose();

        protected void OnEnable() => _input.OnEnable();
        protected void OnDisable() => _input.OnDisable();

        #endregion

        #region Methods

        public void SetInsertStuff(IInsertable insertable, IInsertFriendly insertFriendly)
        {
            _insertable = insertable;
            _insertFriendly = insertFriendly;
        }
        public void TryInsert()
        {
            if (_insertable != null && _insertFriendly!= null)
            {
                _insertFriendly.Insert(_insertable); 
                _insertable.Drop();
            }
        }

        #endregion

        #region Classes

        public class Presenter
        {
            protected readonly Inserter _inserter;

            public Presenter(Inserter inserter) => _inserter = inserter;

            public void Insert() => _inserter.TryInsert();
        }

        #endregion
    }
}
