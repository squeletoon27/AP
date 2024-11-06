using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP3
{
    internal class Program
    {
        public struct medicament
        {
            public string lettre;       //Code identifiant au médicament
            public string nom;          //Nom du médicament
            public int qt;              //Quantité en stock (peut-être supérieure à zéro en début de programme)
            public int qtRav;           //Quantité ajoutée par un ravitaillement unique
            public Boolean dejaRav;     //Booléen indiquant si le médicament a déjà été ravitaillé
        }
        static void Main(string[] args)
        {

            //initialisation des données
            medicament[] tMED = new medicament[4];

            tMED[0].lettre = "D";
            tMED[0].nom = "Doliprane";
            tMED[0].qt = 0;
            tMED[0].qtRav = 0;
            tMED[0].dejaRav = false;

            tMED[1].lettre = "E";
            tMED[1].nom = "Efferalgan";
            tMED[1].qt = 0;
            tMED[1].qtRav = 0;
            tMED[1].dejaRav = false;

            tMED[2].lettre = "I";
            tMED[2].nom = "Ibuprofène";
            tMED[2].qt = 0;
            tMED[2].qtRav = 0;
            tMED[2].dejaRav = false;

            tMED[3].lettre = "S";
            tMED[3].nom = "Spasfon";
            tMED[3].qt = 7;
            tMED[3].qtRav = 0;
            tMED[3].dejaRav = false;

            string reponse = "";            //Fil de caractères servant à contenir la réponse de l'utilisateur pour chaque question
            Boolean Active = true;          //Booléen permettant de maintenir le programme allumé
            Boolean AM = false;             //Booléen permettant de rester sur la section "Ajout de médicaments"
            Boolean AMF = false;            //Booléen permettant de rester sur la fin de l' "Ajout de médicaments"
            Boolean VM = false;             //Booléen permettant d'activer la section "Vente de médicaments"
            Boolean trouve = false;         //Booléen de recherche
            int medSelec = 0;               //Entier contenant l'indice du médicament sélectionné
            int longTMED = (tMED.Length);   //Entier contenant la longueur du tableau tMED
            int totalRav = 0;               //Entier contenant la somme totale d'un ravitaillement unique
            int qtVente = 0;                //Entier contenant la valeur de quantité d'une vente unique
            Single prixUnit =  0;           //Décimal contenant la valeur du prix unitaire d'un médicament pour une vente unique

            while (Active)
            {
                Console.WriteLine("Que voulez-vous faire? AM/VM/VS");   //Option entre les trois sections
                reponse = Console.ReadLine();
                if (reponse == "AM")
                {
                    AM = true;
                    //Initialisation des variables utilisées par la section "Ajout de médicaments en stock"
                    for (int i = 0; i < longTMED; i++)
                    {
                        tMED[i].dejaRav = false;
                        tMED[i].qtRav = 0;
                    }

                    while (AM)  //Maintient la section "Ajout de médicaments en stock" active tant qu'AM est vraie.
                    {
                        Console.WriteLine("Quel type de médicament voulez-vous ajouter au stock? STOP pour arrêter.");
                        reponse = Console.ReadLine();

                        if (reponse == "STOP")
                        {
                            //Calcul et affichage du récapitulatif du ravitaillement
                            Console.WriteLine("Récapitulatif du ravitaillement.");
                            totalRav = 0;
                            for (int i = 0; i < longTMED; i++)
                            {
                                if (tMED[i].qtRav > 0)
                                {
                                    totalRav += tMED[i].qtRav;
                                    Console.WriteLine("Ravitaillement de " + tMED[i].nom + " : " + tMED[i].qtRav);
                                    Console.WriteLine();
                                }
                            }
                            Console.WriteLine("Total de médicaments du ravitaillement : " + totalRav);
                            Console.WriteLine("Retour au menu principal.");
                            Console.ReadLine();
                            AM = false; //Permet de retourner au menu principal en éteignant la section "Ajout de médicaments en stock"
                        }
                        else if (reponse.Length == 1) //Si la variable réponse contient bien une lettre
                        {
                            //Vérification de l'existence du médicament dans la base de données
                            medSelec = 0;
                            trouve = false;
                            while (medSelec < longTMED && !trouve)
                            {
                                if (reponse == tMED[medSelec].lettre)
                                {
                                    trouve = true;
                                    ;
                                }
                                else medSelec++;
                            }
                            //Vérification si le médicament sélectionné n'a pas déjà été ravitaillé
                            
                            if (trouve && tMED[medSelec].dejaRav == false)
                            {
                                AMF = true;
                                while (AMF)
                                {
                                    //Insertion de la quantité du médicament à ravitailler
                                    Console.WriteLine("Quelle est la quantité de " + tMED[medSelec].nom + " qui doit être ajoutée au stock? STOP pour annuler");
                                    reponse = Console.ReadLine();
                                    if (reponse == "STOP") AMF = false;
                                    else
                                    {
                                        try
                                        {
                                            tMED[medSelec].qt += int.Parse(reponse); //Ajoute la quantité au stock déjà existant
                                            tMED[medSelec].qtRav = int.Parse(reponse); //Mémorise la quantité de ce ravitaillement
                                            tMED[medSelec].dejaRav = true; //Active la condition "Déjà ravitaillé", empêchant de ravitailler le médicament deux fois en un seul ravitaillement

                                            Console.WriteLine("Stock actuel de " + tMED[medSelec].nom + " : " + tMED[medSelec].qt);
                                            Console.ReadLine();
                                            medSelec = 0;
                                            AMF = false;
                                        }
                                        catch
                                        {
                                            Console.WriteLine("La quantité saisie doit être un entier");
                                        }
                                    }
                                }

                            }
                            else if (trouve && tMED[medSelec].dejaRav) //Si le médicament a déjà été ravitaillé, message d'erreur
                            {
                                Console.WriteLine("Le stock de " + tMED[medSelec].nom + " a déjà été ravitaillé, veuillez sélectionner un autre médicament.");
                                Console.ReadLine();
                            }
                            else //Si la lettre insérée est inconnue, message d'erreur
                            {
                                Console.WriteLine("Aucun médicament de la base de données ne correspond à la lettre insérée.");
                                Console.ReadLine();
                            }
                        }
                        else //Si la réponse insérée par l'utilisateur ne correspond pas à une lettre ou STOP
                        {
                            Console.WriteLine("Veuillez saisir une lettre (D = Doliprane, E = Efferalgan, I = Ibuprofène, S = Spasfon)");
                            Console.ReadLine();
                        }

                }
                        
                        
                    }

                else if (reponse == "VM") // Début de la section "Vente de médicaments"
                {
                    VM = true;
                    while (VM)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Que voulez-vous vendre? STOP pour arrêter");
                        reponse = Console.ReadLine();
                        if (reponse == "STOP")// Désactivation de la section "Vente de médicaments"
                        {
                            VM = false;
                        }

                        else if (reponse.Length == 1) // Si une lettre est insérée
                        {
                            medSelec = 0;
                            trouve = false;
                            while (medSelec < longTMED && !trouve) // Recherche du médicament
                            {
                                if (reponse == tMED[medSelec].lettre) trouve = true; else medSelec++; //Si le médicament est trouvé
                            }
                            if (trouve)
                            {
                                if (tMED[medSelec].qt == 0) // Si la quantité du médicament trouvé est nulle...
                                {
                                    Console.WriteLine("Pas de " + tMED[medSelec].nom + " en stock."); // ...message d'erreur
                                }
                                else
                                {
                                    //Insertion de la quantité du médicament à vendre
                                    Console.WriteLine("Quelle est la quantité de " + tMED[medSelec].nom + " qui doit être vendue?");
                                    reponse = Console.ReadLine();
                                    try //Vérification du type inséré pour la quantité vendue
                                    {
                                        qtVente = int.Parse(reponse); 
                                        if (tMED[medSelec].qt - qtVente < 0) Console.WriteLine("La quantité vendue ne peut pas être supérieure au stock");
                                        else//Si la qté vendue dépasse le stock, message d'erreur
                                        {
                                            Console.WriteLine("À quel prix sera vendu une unité de " + tMED[medSelec].nom + "?");
                                            try // Vérification du type inséré pour le prix unitaire
                                            {
                                                prixUnit = Single.Parse(Console.ReadLine());
                                                tMED[medSelec].qt -= qtVente; //Soustrait au stock la quantité à vendre
                                                Console.WriteLine("Vente de " + qtVente + " boîtes de " + tMED[medSelec].nom + " à " + prixUnit + " euros la boîte, pour un total de " + qtVente * prixUnit);

                                                Console.WriteLine("Stock actuel de " + tMED[medSelec].nom + " : " + tMED[medSelec].qt);
                                                Console.ReadLine();
                                                medSelec = 0;
                                                qtVente = 0; //Réinitialisation des valeurs clés
                                                prixUnit = 0;
                                            }
                                            catch
                                            {
                                                Console.WriteLine("Saisie du prix incorrecte.");
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        Console.WriteLine("La quantité doit être un entier.");
                                    }
                                }

                            }
                            else Console.WriteLine("Entrée inconnue, veuillez insérer une seule lettre homologuée dans la liste des médicaments");
                        }
                }
                }


                else if (reponse == "VS") //Option visualisation stock
                {
                    Console.WriteLine();
                    Console.WriteLine("Stock actuel de médicaments");
                    for (int i = 0; i<longTMED; i++) //Parcours du tableau
                    {
                        Console.WriteLine("Stock actuel de " + tMED[i].nom + " : " + tMED[i].qt);
                    }
                    Console.ReadLine();


                }
                else if (reponse == "STOP") //Début de la désactivation du programme
                {
                    Console.WriteLine("Au revoir");
                    Console.ReadLine();
                    Active = false;   //Programme éteint
                }
                else Console.WriteLine("Option incorrecte, veuillez recommencer.");
            }
        }
    }
}
