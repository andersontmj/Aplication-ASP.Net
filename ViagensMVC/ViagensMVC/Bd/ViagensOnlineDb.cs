using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using ViagensMVC.Models;

namespace ViagensMVC.Bd
{
    public class ViagensOnlineDb: DbContext
    {
        private const string conexao = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=c:\Users\di3910\Documents\GitHub\Aplication-ASP.Net\ViagensMVC\ViagensMVC\App_Data\ViagensOnlineDb.mdf;Integrated Security=True";

        public ViagensOnlineDb() : base(conexao)
        {

        }

        public DbSet<Destino> Destino { get; private set; }
        public object Destinos { get; internal set; }
    }
}