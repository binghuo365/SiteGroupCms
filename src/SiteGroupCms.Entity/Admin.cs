using System;
namespace SiteGroupCms.Entity
{
    /// <summary>
    /// 管理员-------信息映射实体
    /// </summary>

    public class Admin
    {
        public Admin()
        {
        }
        private int _id;
        private string _username;
        private string _truename;
        private string _AdminPass;
        private string _lastloginip;
        private DateTime _lastlogintime;
        private int _islock;
        private DateTime _addtime;
        private int _siteid;
        private string _rights;
        private int _roleid;
        private int _deptid;
        private string _deptname;
        private string _sex;
        private string _job;
        private string _email;
        private string _telphone;
        private string _mobilephone;
        private int _currentsite;
        private string _imgurl;
        private int _sort;
        private string _catalogid;

        public string Catalogid
        {
            get { return _catalogid; }
            set { _catalogid = value; }
        }

        public int Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }

        public string Imgurl
        {
            get { return _imgurl; }
            set { _imgurl = value; }
        }
        /// <summary>
        /// 管理员的ID
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        ///  登录名名称
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }

        /// <summary>
        /// 管理员实际名称
        /// </summary>
        public string TrueName
        {
            set { _truename = value; }
            get { return _truename; }
        }
        /// <summary>
        /// 管理员权限值，比如:1-1,1-2
        /// </summary>
        public string Rights
        {
            set { _rights = value; }
            get { return _rights; }
        }
        /// <summary>
        /// 管理员密码32加密后
        /// </summary>
        public string Password
        {
            set { _AdminPass = value; }
            get { return _AdminPass; }
        }

        /// <summary>
        /// 最后登录ip
        /// </summary>
        public string LastLoginIp
        {
            set { _lastloginip = value; }
            get { return _lastloginip; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }

        /// <summary>
        /// 管理员状态
        /// </summary>
        public int islock
        {
            set { _islock = value; }
            get { return _islock; }
        }
        /// <summary>
        /// 管理员添加时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 管理员所属siteid  
        /// </summary>
        public int SiteId
        {
            set { _siteid = value; }
            get { return _siteid; }
        }
        /// <summary>
        /// 管理员角色id
        /// </summary>
        public int RoleId
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 管理员部门id
        /// </summary>
        public int DeptId
        {
            set { _deptid = value; }
            get { return _deptid; }
        }
        /// <summary>
        /// 管理员部门名称
        /// </summary>
        public string DeptName
        {
            set { _deptname = value; }
            get { return _deptname; }
        }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 管理员职位
        /// </summary>
        public string Job
        {
            set { _job = value; }
            get { return _job; }
        }
        /// <summary>
        /// 管理员email
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 管理员电话
        /// </summary>
        public string Telphone
        {
            set { _telphone = value; }
            get { return _telphone; }
        }
        /// <summary>
        /// 管理员手机
        /// </summary>
        public string MobilePhone
        {
            set { _mobilephone = value; }
            get { return _mobilephone; }
        }
        /// <summary>
        /// 对于超级管理员来说 当前的操作网站id
        /// </summary>
        public int CurrentSite
        {
            set { _currentsite = value; }
            get { return _currentsite; }
        }
    }
}
