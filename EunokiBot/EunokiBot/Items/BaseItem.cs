using Discord.Commands;

using EunokiBot.Model;

namespace EunokiBot.Items
{
    public abstract class BaseItem
    {
        public void Use(SocketCommandContext context, User user, Inventory inventory, object param = null)
        {
            OnItemUse(context, user, inventory, param);
            RemoveItem(inventory);
        }

        protected abstract void OnItemUse(
            SocketCommandContext context, User user, Inventory inventory, object param = null);

        protected void RemoveItem(Inventory inventory)
        {
            ItemIDAttribute attr = (ItemIDAttribute)GetType().GetCustomAttributes(true)[0];
            inventory.RemoveItem(attr.ID);
        }
    }
}
