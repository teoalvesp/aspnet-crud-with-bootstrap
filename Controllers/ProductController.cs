using Microsoft.EntityFrameworkCore;
using CRUD_AspNet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace CRUD_AspNet.Controllers
{

    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ação para listar os produtos
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync(); // Obtém todos os produtos do banco de dados
            return View(products); // Retorna a view com os produtos
        }


        // Exibe a página de criação do produto
        public IActionResult Create()
        {
            return View();
        }

        // Ação para gravar o novo produto no banco de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Adiciona o produto ao banco de dados
                    _context.Add(product);
                    await _context.SaveChangesAsync();

                    // Redireciona para a lista de produtos após a criação
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Em caso de erro ao salvar, podemos exibir o erro na view ou logar
                    ModelState.AddModelError("", "Ocorreu um erro ao criar o produto: " + ex.Message);
                }
            }

            // Se o ModelState não for válido, retorna para a view de criação com os dados
            return View(product);
        }

        // Exibe a página de atualização do produto
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Ação para atualizar o produto no banco de dados
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("Id, Name, Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound(); // Verifica se o ID do produto está correto
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualiza o produto no banco de dados
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    // Redireciona para a lista de produtos após a atualização
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound(); // Retorna erro se o produto não existir
                    }
                    else
                    {
                        throw; // Lança exceção se outro erro ocorrer
                    }
                }
            }

            // Se o ModelState não for válido, retorna para a view de atualização com os erros
            return View(product);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Action to confirm the deletion of the product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


    }


}