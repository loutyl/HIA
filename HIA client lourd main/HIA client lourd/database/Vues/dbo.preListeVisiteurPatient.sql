CREATE VIEW [dbo].[preListeVisiteurPatient]
	AS SELECT Visiteur.* , Patient.nom_patient, Patient.prenom_patient
	FROM Visiteur, Visiter, Patient 
	WHERE Visiteur.id_visiteur = Visiter.id_visiteur
	AND visiter.id_patient = Patient.id_patient
	AND Visiteur.num_visite = Patient.num_visite