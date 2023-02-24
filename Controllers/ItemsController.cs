using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StuManSys.Data;
using StudentManagementSys.Model;
using AutoMapper;
using StudentManagementSys.Controllers.Dto;

namespace StuManSys.Controllers
{
    public class ItemsController : Controller
    {
        private readonly StuManSysContext _context;

        public ItemsController(StuManSysContext context)
        {
            _context = context;
        }

        // AutoMapper configuration
        private MapperConfiguration config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Item, ItemDto>()
        );

        private MapperConfiguration configReversed = new MapperConfiguration(cfg =>
                    cfg.CreateMap<ItemDto, Item>()
        );


        // GET: Items
        public async Task<IActionResult> Index()
        {
              //return _context.Item != null ? 
              //            View(await _context.Item.ToListAsync()) :
              //            Problem("Entity set 'StuManSysContext.Item'  is null.");

            if (_context.Item != null)
            {
                var mapper = new Mapper(config);
                List<ItemDto> rs = new List<ItemDto>();
                List<Item> lsItem = await _context.Item.ToListAsync();
                foreach (Item i in lsItem)
                {
                    rs.Add(mapper.Map<ItemDto>(i));
                }
                return View(rs);
            }
            else
            {
                return Problem("Entity set 'StuManSysContext.Item'  is null.");
            }
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }
            var mapper = new Mapper(config);
            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemID == id);
            ItemDto itemDto = mapper.Map<ItemDto>(item);
            if (item == null)
            {
                return NotFound();
            }

            return View(itemDto);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Desc,ItemID,price")] ItemDto itemDto)
        {
            //if (ModelState.IsValid)
            var item = new Mapper(configReversed).Map<Item>(itemDto);
            {
                _context.Add(item);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            ItemDto itemDto = new Mapper(config).Map<ItemDto>(item);
            if (item == null)
            {
                return NotFound();
            }
            return View(itemDto);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Desc,ItemID,price")] ItemDto itemDto)
        {
            if (id != itemDto.ItemID)
            {
                return NotFound();
            }
            Item item = new Mapper(configReversed).Map<Item>(itemDto);
            //if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Item == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.ItemID == id);
            ItemDto itemDto = new Mapper(config).Map<ItemDto>(item);
            if (item == null)
            {
                return NotFound();
            }

            return View(itemDto);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Item == null)
            {
                return Problem("Entity set 'StuManSysContext.Item'  is null.");
            }
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                _context.Item.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(string id)
        {
          return (_context.Item?.Any(e => e.ItemID == id)).GetValueOrDefault();
        }
    }
}
