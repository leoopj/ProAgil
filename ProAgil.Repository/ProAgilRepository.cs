using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _context;
        public ProAgilRepository(ProAgilContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        //Evento
        public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query.OrderByDescending(q => q.DataEvento);

                return await query.ToArrayAsync();
        }

        public async Task<Evento> GetAllEventoAsyncById(int eventoId, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query
                    .OrderByDescending(q => q.DataEvento)
                    .Where(x => x.EventoId == eventoId);

                return await query.FirstOrDefaultAsync();
        }

        public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(x => x.Lotes)
                .Include(x => x.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(p => p.Palestrante);
                }

                query = query
                    .OrderByDescending(q => q.DataEvento)
                    .Where(x => x.Tema.Contains(tema));

                return await query.ToArrayAsync();
        }

        //Palestrantes
        public async Task<Palestrante> GetPalestrantesAsyncById(int palestranteId, bool incluirEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(x => x.RededesSociais);

                if(incluirEventos)
                {
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.OrderBy(x => x.Nome)
                    .Where(x => x.Id == palestranteId);
                    

                return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string nome, bool incluirEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(x => x.RededesSociais);

                if(incluirEventos)
                {
                    query = query
                        .Include(pe => pe.PalestrantesEventos)
                        .ThenInclude(e => e.Evento);
                }

                query = query.OrderBy(x => x.Nome)
                    .Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
                    

                return await query.ToArrayAsync();
        }

    }
}