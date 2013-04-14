namespace Incoding.Data
{
    #region << Using >>

    using System;

    #endregion

    public interface IManagerDataBase
    {
        void Create();

        void Drop();

        void Update();

        bool IsExist();

        bool IsExist(out Exception outException);
    }
}