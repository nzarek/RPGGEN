using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
/*
 * Nicholas Zarek
 * Final Project
 * 
 * 
 * This program demonstrates the use of inheritance, dynamic data structures, and database access/management.
 *Resource Used: forums.asp.net/t/1677937.aspx?Insert+data+from+textbox+to+mdf+database+c+
 */
namespace Zarek_FinalProject
{
    class Character //base class
    {
        public string characterName;
        public string characterClass;
        public int health = 5;
        public int magic = 5;
        public int strength = 5;
        public int dexterity = 5;
        public int intelligence = 5;

        public void printBase()
        {
            Console.WriteLine("Name: " + characterName);
            Console.WriteLine("Class: " + characterClass);
            Console.WriteLine("Health: " + health);
            Console.WriteLine("Magic: " + magic);
            Console.WriteLine("Strength: " + strength);
            Console.WriteLine("Dexterity: " + dexterity);
            Console.WriteLine("Intelligence: " + intelligence);
        }

    }
    //derived classes below
    class defaultCharacter : Character
    {

        public void defaultCharacterAttributes()
        {
            base.health = base.health;
            base.magic = base.magic;
            base.strength = base.strength;
            base.dexterity = base.dexterity;
            base.intelligence = base.intelligence;

        }
    }

    class Warrior : Character
    {
        public void WarriorAttributes()
        {
            base.health += 3;
            base.magic -= 3;
            base.strength += 4;
            base.dexterity += 2;
            base.intelligence -= 3;
        }
    }

    class Rogue : Character
    {
        public void RogueAttributes()
        {
            base.health += 1;
            base.magic -= 1;
            base.strength = base.strength;
            base.dexterity += 3;
            base.intelligence += 1;
        }
    }

    class Wizard : Character
    {
        public void WizardAttributes()
        {
            base.health -= 1;
            base.magic += 3;
            base.strength -= 3;
            base.dexterity -= 1;
            base.intelligence += 5;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Stack myStack = new Stack(); //new stack
            

            Warrior w1 = new Warrior();
            w1.characterName = "Michael McTimothy";
            w1.characterClass = "Warrior";
            w1.WarriorAttributes();
            w1.printBase();
            Console.WriteLine();
            myStack.Push(w1.characterName); //push character names to stack

            Rogue r1 = new Rogue();
            r1.characterName = "Taman Windriver";
            r1.characterClass = "Rogue";
            r1.RogueAttributes();
            r1.printBase();
            Console.WriteLine();
            myStack.Push(r1.characterName);

            Wizard w2 = new Wizard();
            w2.characterName = "Mara Brightwood";
            w2.characterClass = "Wizard";
            w2.WizardAttributes();
            w2.printBase();
            Console.WriteLine();
            myStack.Push(w2.characterName);

            defaultCharacter dC = new defaultCharacter();
            dC.characterName = "Default";
            dC.characterClass = "Default";
            dC.defaultCharacterAttributes();
            dC.printBase();
            Console.WriteLine();
            myStack.Push(dC.characterName);

            Console.WriteLine("Character Names:");
            foreach (var itm in myStack)
                Console.WriteLine(itm);
            Console.WriteLine();

            Console.WriteLine("Rename your Default Character:");
            myStack.Pop(); //pop default character name
            dC.characterName = Console.ReadLine();
            myStack.Push(dC.characterName); //push new character name to stack
            Console.WriteLine();

            Console.WriteLine("Updated Character Names:");
            foreach (var itm in myStack)
                Console.WriteLine(itm);
            Console.WriteLine();

            Console.WriteLine("Press enter to save:");

            //write character names to database
            SqlConnection conn1 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ZarekFinalDB.mdf;Integrated Security=True;Connect Timeout=30");

            SqlCommand addSql = new SqlCommand("INSERT INTO [Table] (Names) VALUES (@Names)", conn1);
            conn1.Open();
            SqlParameter nameParam = addSql.Parameters.AddWithValue("@Names", System.Data.SqlDbType.NVarChar);
            nameParam.Value = w1.characterName;
            addSql.ExecuteNonQuery();
            nameParam.Value = r1.characterName;
            addSql.ExecuteNonQuery();
            nameParam.Value = w2.characterName;
            addSql.ExecuteNonQuery();
            nameParam.Value = dC.characterName;
            addSql.ExecuteNonQuery();
            conn1.Close();

            Console.WriteLine("Printing From Database:");
            Console.WriteLine();
            //read character names from database
            SqlConnection conn2 = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ZarekFinalDB.mdf;Connect Timeout=30");

            SqlCommand showSql = new SqlCommand("SELECT * From [Table]", conn2);
            conn2.Open();
            SqlDataReader reader = showSql.ExecuteReader();
            
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.WriteLine(reader.GetValue(i));
                    }
                    Console.WriteLine();
                }

            conn2.Close();
            Console.ReadLine();
            Console.ReadKey();
        }

    }
}


