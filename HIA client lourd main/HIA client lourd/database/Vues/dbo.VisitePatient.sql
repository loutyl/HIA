CREATE VIEW [dbo].[VisitePatient]
	AS SELECT Visiteur.nom_visiteur, Visiteur.prenom_visiteur, Visiter.num_bon_visite, Visiter.date_deb_visite,
	Visiter.date_fin_visite, Visiter.status_demande
	FROM Visiteur, Visiter, Patient
	WHERE Visiteur.id_visiteur = Visiter.id_visiteur AND Visiter.id_patient = Patient.id_patient