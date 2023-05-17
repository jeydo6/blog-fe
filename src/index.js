const appMainContent = document.querySelector('.app-main-content');

const appNavLinkActiveClassName = 'nav-link-active';
const appNavLinks = document.querySelectorAll('a.app-main-navbar-nav-link');

appNavLinks.forEach(navLink =>
	navLink.addEventListener('click', navLinkEventListener));

loadDefaultContent();

function navLinkEventListener(e) {
	e.preventDefault();
	loadContent(e.target);
}

function loadDefaultContent() {
	const appNavLinkActive = getAppNavLinkActive();
	if (appNavLinkActive) {
		loadContent(appNavLinkActive);
	}
}

function loadContent(navLink) {
	fetch(navLink.href)
		.then(response => response.text())
		.then(html => {
			const appNavLinkActive = getAppNavLinkActive();
			if (appNavLinkActive) {
				appNavLinkActive.classList.remove(appNavLinkActiveClassName);
			}
			appMainContent.innerHTML = html;
			navLink.classList.add(appNavLinkActiveClassName);
		});
}

function getAppNavLinkActive() {
	return document.querySelector(`.${appNavLinkActiveClassName}`);
}
