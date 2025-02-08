using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace ShoppingListApp
{
    public partial class MainWindow : Window
    {
        private List<ShoppingItem> shoppingItems = new List<ShoppingItem>();
        private const string DataFilePath = "shopping_list_data.json";

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(DataFilePath))
            {
                try
                {
                    using (var stream = new FileStream(DataFilePath, FileMode.Open))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(List<ShoppingItem>));
                        shoppingItems = (List<ShoppingItem>)serializer.ReadObject(stream);
                        RefreshShoppingList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveData()
        {
            try
            {
                using (var stream = new FileStream(DataFilePath, FileMode.Create))
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<ShoppingItem>));
                    serializer.WriteObject(stream, shoppingItems);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            string name = ItemNameTextBox.Text?.Trim();
            if (!int.TryParse(ItemQuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Please fill all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            shoppingItems.Add(new ShoppingItem { Name = name, Quantity = quantity, Category = category });
            RefreshShoppingList();
            ClearInputFields();
            SaveData();
            MessageBox.Show("Item added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = ShoppingListView.SelectedItem as ShoppingItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string name = ItemNameTextBox.Text?.Trim();
            if (!int.TryParse(ItemQuantityTextBox.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(category))
            {
                MessageBox.Show("Please fill all fields correctly.\