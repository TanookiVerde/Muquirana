using System.Collections;

public interface IItem {
	void Effect();
	void Destroy();
	int GetValue();
	string GetDescription();
	IEnumerator CatchAnimation();
	IEnumerator OnTouch();
	IEnumerator OnFall();
}
