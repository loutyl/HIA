CREATE VIEW [dbo].[LocalisationPatient]
	AS SELECT Patient.nom_patient, Patient.prenom_patient, Patient.age_patient, ServicesH.nom_service, Etage.etage,
	Chambre.num_chambre, Lit.nom
	FROM Patient, Accueillir, Accueil, Occuper, Lit, Appartenir, Chambre, Comporter, ServicesH, Affecter, Etage
	WHERE Patient.id_patient = Accueillir.id_patient
	AND Accueillir.id_accueil = Accueil.id_accueil
	AND Accueil.id_accueil = Occuper.id_accueil
	AND Occuper.id_lit = Lit.id_lit
	AND Lit.id_lit = Appartenir.id_lit
	AND Appartenir.id_chambre = Chambre.id_chambre
	AND Chambre.id_chambre = Comporter.id_chambre
	AND Comporter.id_service = ServicesH.id_service
	AND ServicesH.id_service = Affecter.id_service
	AND Affecter.id_etage = Etage.id_etage
	AND Patient.nom_patient = 'MAALEM';
